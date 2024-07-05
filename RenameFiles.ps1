$oldstring = $args[0]
$newstring = $args[1]

if (($oldstring -eq "") -OR ($newstring -eq "")) {
    Write-Host "Usage: .\script.ps1 <old string> <new string>"
    exit 1
}

$directory = "."

# Replace string in file and folder names
Get-ChildItem -Path $directory -Recurse -Exclude .git | Where-Object { $_.Name -like "*$oldstring*" } | ForEach-Object {
    $newName = $_.Name -replace [regex]::Escape($oldstring), $newstring
    Rename-Item -Path $_.FullName -NewName $newName
}

# Replace string in file contents
Get-ChildItem -Path $directory -Recurse -File -Include *.cs,*.csproj,*.json,*.sln | ForEach-Object {
    (Get-Content -Path $_.FullName) -replace [regex]::Escape($oldstring), $newstring | Set-Content -Path $_.FullName
}
