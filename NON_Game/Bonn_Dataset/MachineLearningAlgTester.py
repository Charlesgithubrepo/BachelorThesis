# ========================
# IMPORTS
# ========================
import os
import os.path as op
from download import download
import numpy as np
import pandas as pd
import time
from sklearn.ensemble import RandomForestClassifier
from sklearn.neighbors import KNeighborsClassifier
from sklearn.svm import SVC
from sklearn.metrics import f1_score, confusion_matrix, accuracy_score
from sklearn.model_selection import StratifiedKFold, cross_val_score, GridSearchCV, cross_val_predict
from sklearn.pipeline import Pipeline
from sklearn.preprocessing import StandardScaler
from mne_features.feature_extraction import FeatureExtractor
from joblib import dump


print(__doc__)

# ========================
# DATA DOWNLOAD
# ========================

def download_bonn_ieeg(path, verbose=False):
    """
    Function to download the Bonn iEEG dataset.
    :param path: Path to download the data to
    :param verbose: Flag to enable verbose logging
    :return: List of paths to the downloaded files
    """
    base_url = 'https://www.ukbonn.de/epileptologie/arbeitsgruppen/ag-lehnertz-neurophysik/downloads/'
    urls = [('setA', 'z.zip'), ('setB', 'o.zip'), ('setC', 'n.zip'), ('setD', 'f.zip'), ('setE', 's.zip')]
    paths = list()
    for set_name, url_suffix in urls:
        _path = download(op.join(base_url, url_suffix),
                         op.join(path, set_name), kind='zip', replace=False,
                         verbose=verbose)
        paths.append(_path)
    return paths


# Download the data to a specific path
current_directory = os.getcwd()
paths = download_bonn_ieeg(current_directory)

# ========================
# DATA LOADING
# ========================

# Read the data from .txt files. Only the iEEG epochs in ./Bonn_Dataset/setE correspond to ictal activity.
data_segments = list()
labels = list()
sfreq = 173.61 # Sampling frequency
for path in paths:
    fnames = [s for s in os.listdir(path) if s.lower().endswith('.txt')] # Only load .txt files
    for fname in fnames:
        _data = pd.read_csv(op.join(path, fname), header=None)
        data_segments.append(_data.values.T[None, ...])
    if 'setE' in path:
        labels.append(np.ones((len(fnames),))) # Ictal activity
    else:
        labels.append(np.zeros((len(fnames),))) # Non-ictal activity

data = np.concatenate(data_segments)
y = np.concatenate(labels, axis=0)

# Print the shape of the extracted data
print(data.shape)

# ========================
# CLASSIFICATION TASK PREPARATION
# ========================

# Define the classifiers, parameters, and selected functions
selected_funcs = ['line_length', 'kurtosis', 'ptp_amp', 'skewness']

classifiers_and_parameters = [
    ('SVM with Linear Kernel', SVC(kernel='linear', random_state=42), {}),
    ('SVM with RBF Kernel', SVC(kernel='rbf', random_state=42), {'clf__C': [0.1, 1, 10], 'clf__gamma': ['scale', 'auto']}),
    ('K-Nearest Neighbors', KNeighborsClassifier(), {'clf__n_neighbors': [1, 3, 5, 7, 9, 11, 13, 15, 17, 19],
                                                      'clf__weights': ['uniform', 'distance'], 'clf__p': [1, 2]}),
    ('Random Forest', RandomForestClassifier(random_state=42), {'clf__n_estimators': [50, 100], 'clf__max_depth': [2, 4, 6]})
]

# Perform cross-validation with GridSearchCV
skf = StratifiedKFold(n_splits=3, shuffle=True, random_state=42)

for algorithm_name, classifier, param_grid in classifiers_and_parameters:
    pipeline = Pipeline([
        ('fe', FeatureExtractor(sfreq=sfreq, selected_funcs=selected_funcs)), # Feature extraction
        ('scaler', StandardScaler()), # Scaling the features
        ('clf', classifier) # Applying the classifier
    ])

    grid_search = GridSearchCV(pipeline, param_grid=param_grid, cv=skf, scoring='accuracy')
    grid_search.fit(data, y)

    # Get the best estimator (pipeline with best hyperparameters)
    best_pipeline = grid_search.best_estimator_

    # Measure the time to fit the best pipeline
    start_time_fit = time.time()
    best_pipeline.fit(data, y)
    elapsed_time_fit = time.time() - start_time_fit

    # Measure the time to predict using the best pipeline
    start_time_predict = time.time()
    y_pred = best_pipeline.predict(data)
    elapsed_time_predict = time.time() - start_time_predict

    # Print the best hyperparameters, confusion matrix, scores, and timing information
    print(f'Algorithm: {algorithm_name}')
    print('Best Hyperparameters:', grid_search.best_params_)
    print('Confusion Matrix:')
    print(confusion_matrix(y, y_pred))
    print('Best F1 Score: {:.3f}'.format(f1_score(y, y_pred)))
    print('Best Accuracy: {:.3f}'.format(accuracy_score(y, y_pred)))
    print('Best Cross-Validation Accuracy: {:.3f}'.format(grid_search.best_score_))
    print(f'Time to Fit: {elapsed_time_fit:.5f} seconds')
    print(f'Time to Predict: {elapsed_time_predict:.5f} seconds')
    print('---')

    if algorithm_name == 'SVM with Linear Kernel':
        # Save the best SVM linear kernel model to a file
        dump(best_pipeline, 'svm_linear_kernel_model.joblib')

print("Training complete.")