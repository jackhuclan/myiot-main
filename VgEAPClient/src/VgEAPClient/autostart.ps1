#定义程序名称和路径
$programName = "VgEAPClient"
$currentDir = Get-Location
$programPath = "$currentDir\VgEAPClient.exe"

#获取启动文件夹路径
$startupFolder = [System.Environment]::GetFolderPath('Startup')

#创建快捷方式的路径
$shortcutPath = "$startupFolder\$programName.lnk"

#创建wScript.ShelL对象
$wshShell = New-Object -ComObject WScript.Shell

#创建快捷方式
$shortcut = $wshShell.CreateShortcut($shortcutPath)
$shortcut.TargetPath = $programPath
$shortcut.Save()
