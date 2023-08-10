import numpy as np
import pandas as pd
from sklearn.utils import shuffle

# Load datasets
ictal_data = pd.read_csv('Data\Plot_Seizure_Bonn\setE\S001.txt', header=None)
non_ictal_data = pd.read_csv('Data\Plot_Seizure_Bonn\setA\Z001.txt', header=None)

# Segment size
segment_size = 100  # Adjust according to your needs

# Cut the ictal and non-ictal data into segments
ictal_segment_indices = list(range(0, len(ictal_data), segment_size))
non_ictal_segment_indices = list(range(0, len(non_ictal_data), segment_size))

# Replace some non-ictal segments with ictal segments
replacement_rate = 0.5  # Adjust according to your needs
num_replacements = int(replacement_rate * len(non_ictal_segment_indices))

for _ in range(num_replacements):
    # Randomly select an ictal segment
    ictal_segment_index = np.random.choice(ictal_segment_indices)

    # Randomly select a non-ictal segment to replace
    replace_idx = np.random.choice(len(non_ictal_segment_indices))

    # Replace the non-ictal segment with the ictal one
    non_ictal_segment_indices[replace_idx] = ictal_segment_index

    # Remove the used ictal segment so it's not used again
    ictal_segment_indices.remove(ictal_segment_index)

# Combine the modified non-ictal data and remaining ictal data into a new dataset
all_segment_indices = non_ictal_segment_indices + ictal_segment_indices
new_dataset = pd.concat([ictal_data[i:i + segment_size] for i in all_segment_indices])

# Shuffle and save the new dataset
new_dataset = shuffle(new_dataset)
new_dataset.to_csv('Data/Plot_Seizure_Bonn/CreatedTestdata/new_dataset.txt', index=False, header=False)

