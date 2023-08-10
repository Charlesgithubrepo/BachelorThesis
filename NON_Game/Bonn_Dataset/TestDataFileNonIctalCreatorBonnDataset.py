import os
import glob
import numpy as np
import pandas as pd
from sklearn.utils import shuffle

def create_mixed_file(non_ictal_file_path1, non_ictal_file_path2, output_file_path, segment_size=100, replacement_rate=0.5):
    # Load datasets
    non_ictal_data1 = pd.read_csv(non_ictal_file_path1, header=None)
    non_ictal_data2 = pd.read_csv(non_ictal_file_path2, header=None)

    # Cut the non-ictal data into segments
    non_ictal_segment_indices1 = list(range(0, len(non_ictal_data1), segment_size))
    non_ictal_segment_indices2 = list(range(0, len(non_ictal_data2), segment_size))

    # Replace some non-ictal segments with other non-ictal segments
    num_replacements = int(replacement_rate * len(non_ictal_segment_indices1))

    for _ in range(num_replacements):
        # Randomly select a non-ictal segment
        non_ictal_segment_index2 = np.random.choice(non_ictal_segment_indices2)

        # Randomly select a non-ictal segment to replace
        replace_idx = np.random.choice(len(non_ictal_segment_indices1))

        # Replace the non-ictal segment with the other non-ictal one
        non_ictal_segment_indices1[replace_idx] = non_ictal_segment_index2

        # Remove the used non-ictal segment so it's not used again
        non_ictal_segment_indices2.remove(non_ictal_segment_index2)

    # Combine the modified non-ictal data and remaining non-ictal data into a new dataset
    all_segment_indices = non_ictal_segment_indices1 + non_ictal_segment_indices2
    new_dataset = pd.concat([non_ictal_data1[i:i + segment_size] for i in all_segment_indices])

    # Shuffle and save the new dataset
    new_dataset = shuffle(new_dataset)
    new_dataset.to_csv(output_file_path, index=False, header=False)


non_ictal_folder_path1 = 'setA/'
non_ictal_folder_path2 = 'setB/'
output_folder_path = 'CreatedNonIctalTestdata/'

# Ensure that the output folder exists
os.makedirs(output_folder_path, exist_ok=True)

# Delete all existing files in the directory
files = glob.glob(output_folder_path + '*')
for f in files:
    os.remove(f)

# For each pair of files
for i in range(100):
    non_ictal_file_path1 = os.path.join(non_ictal_folder_path1, f'Z{i+1:03}.txt')
    non_ictal_file_path2 = os.path.join(non_ictal_folder_path2, f'O{i+1:03}.txt')
    output_file_path = os.path.join(output_folder_path, f'new_NonIctal_{i+1:03}.txt')

    create_mixed_file(non_ictal_file_path1, non_ictal_file_path2, output_file_path)
