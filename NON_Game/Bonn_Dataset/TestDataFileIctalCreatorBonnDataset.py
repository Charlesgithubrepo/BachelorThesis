import os
import glob
import numpy as np
import pandas as pd
from sklearn.utils import shuffle

def create_mixed_file(ictal_file_path, non_ictal_file_path, output_file_path, segment_size=100, replacement_rate=0.5):
    # Load datasets
    ictal_data = pd.read_csv(ictal_file_path, header=None)
    non_ictal_data = pd.read_csv(non_ictal_file_path, header=None)

    # Cut the ictal and non-ictal data into segments
    ictal_segment_indices = list(range(0, len(ictal_data), segment_size))
    non_ictal_segment_indices = list(range(0, len(non_ictal_data), segment_size))

    # Replace some non-ictal segments with ictal segments
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
    new_dataset.to_csv(output_file_path, index=False, header=False)


ictal_folder_path = 'setE/'
non_ictal_folder_path = 'setA/'
output_folder_path = 'CreatedIctalTestdata'

# Ensure that the output folder exists
os.makedirs(output_folder_path, exist_ok=True)

# Delete all existing files in the directory
files = glob.glob(output_folder_path + '/*')
for f in files:
    if os.path.isfile(f):
        os.remove(f)

# For each pair of files
for i in range(100):
    ictal_file_path = os.path.join(ictal_folder_path, f'S{i+1:03}.txt')
    non_ictal_file_path = os.path.join(non_ictal_folder_path, f'Z{i+1:03}.txt')
    output_file_path = os.path.join(output_folder_path, f'new_Ictal_{i+1:03}.txt')

    create_mixed_file(ictal_file_path, non_ictal_file_path, output_file_path)
