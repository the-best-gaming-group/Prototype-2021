#!/bin/bash

# Directory containing the .csproj files
# Loop through all .csproj files in the directory
for file in $(find -name '*.csproj'); do
    # Use sed to insert the line after the specified line
    sed -i '/<TargetFrameworkVersion>v4.7.1<\/TargetFrameworkVersion>/a <TargetFramework>NET_STANDARD_2_1<\/TargetFramework>' "$file"
    echo "Updated $file"
done
