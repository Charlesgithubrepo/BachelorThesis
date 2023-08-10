import os
import glob
import shutil
import random

def shuffle_files_to_new_directory(input_folder_paths, output_folder_path, n_files_per_folder):
    # Ensure that the output folder exists
    os.makedirs(output_folder_path, exist_ok=True)

    # Delete all existing files in the directory
    files = glob.glob(os.path.join(output_folder_path, '*'))
    for f in files:
        os.remove(f)

    # Get n_files_per_folder files from each input folder
    file_paths = []
    for input_folder_path in input_folder_paths:
        all_files = glob.glob(os.path.join(input_folder_path, '*.txt'))
        selected_files = random.sample(all_files, n_files_per_folder)
        file_paths.extend(selected_files)

    # Shuffle all the selected files
    random.shuffle(file_paths)

    # Copy the files to the output folder, preserving the shuffle order
    for i, file_path in enumerate(file_paths):
        shutil.copy2(file_path, os.path.join(output_folder_path, f'shuffled_{i+1:03}.txt'))

input_folder_paths = ['CreatedNonIctalTestdata', 'CreatedIctalTestdata']
output_folder_path = 'UnseenData'
n_files_per_folder = 50

shuffle_files_to_new_directory(input_folder_paths, output_folder_path, n_files_per_folder)
