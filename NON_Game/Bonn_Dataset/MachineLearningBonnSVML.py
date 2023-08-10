import os
import pandas as pd
from joblib import load

# Load the saved model
pipeline = load('svm_linear_kernel_model.joblib')

# Specify the directory containing the files to test
directory ='UnseenData'

# Get a list of all .txt files in the directory
file_names = [f for f in os.listdir(directory) if f.endswith('.txt')]

# Initialize counters for ictal and non-ictal classifications
ictal_count = 0
non_ictal_count = 0

# For each file in the directory
for file_name in file_names:
    # Create the full file path
    file_path = os.path.join(directory, file_name)

    # Load the data from the file
    new_data = pd.read_csv(file_path, header=None)

    # Adjust data format to match the training setup
    new_data_segments = new_data.values.T[None, ...]

    # Predict using the model
    prediction = pipeline.predict(new_data_segments)

    # Check if the prediction is ictal or non-ictal and increment the respective counter
    if prediction[0] == 1:
        ictal_count += 1
        print(f'File {file_path} has ictal activity.')
    else:
        non_ictal_count += 1
        print(f'File {file_path} has non-ictal activity.')

# Print the total counts
print(f'Total ictal classified: {ictal_count}')
print(f'Total non-ictal classified: {non_ictal_count}')

print("Testing complete.")
