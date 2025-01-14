#!/usr/bin/env python3
# There's too many places where the version has to be set, so I've made a little script to do it for me each time.

import argparse
import os.path
import re


class FilePatternReplacement:
    def __init__(self, file, pattern, replacement):
        self.file = file
        self.pattern = pattern
        self.replacement = replacement

    def replace(self):
        if not os.path.exists(self.file):
            return f"{self.file} does not exist"

        with open(self.file, 'r', encoding='utf-8') as f:
            content = f.read()
            modified_content = re.sub(self.pattern, self.replacement, content)

        with open(self.file, 'w', encoding='utf-8') as f:
            f.write(modified_content)


if __name__ == '__main__':
    parser = argparse.ArgumentParser(prog='Set new version')
    parser.add_argument('version', nargs='?', type=str)
    args = parser.parse_args()

    proj_name = "ValheimNewItemTemplate"

    version = args.version

    if version is None or version == "":
        parser.print_help()
        sys.exit(1)

    file_pattern_replacements = [
        FilePatternReplacement(f'{proj_name}/{proj_name}.csproj',
                               '<Version>[0-9.]+<\/Version>',
                               f'<Version>{version}.0</Version>'),
        FilePatternReplacement(f'{proj_name}/{proj_name}.cs',
                               'PluginVersion = [".0-9]+',
                               f'PluginVersion = "{version}"'),
        FilePatternReplacement(f'{proj_name}/Package/manifest.json',
                               '"version_number": [".0-9]+',
                               f'"version_number": "{version}"'),
    ]

    errors = [x.replace() for x in file_pattern_replacements]
    for error in errors:
        if error is not None:
            print(error)
