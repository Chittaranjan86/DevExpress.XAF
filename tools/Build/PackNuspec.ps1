param(
    $nugetBin="$PSScriptRoot\..\..\bin\Nupkg",
    $sourceDir="$PSScriptRoot\..\.."
)
$ErrorActionPreference="Stop"
New-Item $nugetBin -ItemType Directory -Force|Out-Null
& (Get-XNugetPath) pack "$sourceDir\Tools\Xpand.VersionConverter\Xpand.VersionConverter.nuspec" -OutputDirectory $nugetBin -NoPackageAnalysis
if ($lastexitcode){
    throw 
}
$packData = [pscustomobject] @{
    nugetBin = $nugetBin
}

set-location $sourceDir
$assemblyVersions=Get-ChildItem "$sourceDir\src" "*.csproj" -Recurse|ForEach-Object{
    $assemblyInfo=get-content "$($_.DirectoryName)\Properties\AssemblyInfo.cs"
    [PSCustomObject]@{
        Name = [System.IO.Path]::GetFileNameWithoutExtension($_.FullName)
        Version =[System.Text.RegularExpressions.Regex]::Match($assemblyInfo,'Version\("([^"]*)').Groups[1].Value
    }
}
Set-Content "$sourceDir\bin\Readme.txt" "BUILD THE PROJECT BEFORE OPENING THE MODEL EDITOR" 
Get-ChildItem "$sourceDir\bin" "*.nuspec" -Recurse|ForEach-Object{
    $packageName=[System.IO.Path]::GetFileNameWithoutExtension($_.FullName)
    $assembly=$assemblyVersions|Where-Object{$_.name -eq $packageName}
    $name=$_.FullName
    $directory=$_.Directory.Parent.FullName
    & (Get-XNugetPath) pack $name -OutputDirectory $($packData.nugetBin) -Basepath $directory -Version $($assembly.Version)
    if ($lastexitcode){
        throw $_.Exception
    }
}

    
