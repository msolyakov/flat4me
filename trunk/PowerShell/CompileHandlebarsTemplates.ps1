<# Check if compiled already exists #>
if(Test-Path compiled)
{
    Remove-Item compiled -Recurse
}
<# Create folder #>
md compiled
<# Copy templates to folder #>
Copy-Item *.cshtml compiled
<# GoTo folder #>
cd compiled
<# Remove cshtml extension #>
Get-ChildItem | ForEach-Object { Rename-Item $_.Name $_.Name.Replace('cshtml', '') }

$templates = Get-ChildItem

foreach($t in $templates)
{
    $c = get-content $t
    <# Remove first and last row (<script> tags exactly) #>
    $cc = $c[1..($c.count - 2)]
    Set-Content $t $cc -Encoding UTF8
}

$templates = Get-ChildItem
foreach($t in $templates)
{
    Write-Output $t.Name <# inform in console #>
    <# Compile each #>
    handlebars $t -f "$t.js" -b -m
    Remove-Item $t
}

"completed"