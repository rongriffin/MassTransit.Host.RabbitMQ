param($installPath, $toolsPath, $package, $project)

if(!$toolsPath){
	$project = Get-Project
}

function Add-StartServiceHostProject {
	[xml] $prjXml = Get-Content $project.FullName
	foreach($PropertyGroup in $prjXml.project.ChildNodes)
	{
		if($PropertyGroup.StartAction -ne $null)
		{
			return
		}
	}

	$propertyGroupElement = $prjXml.CreateElement("PropertyGroup", $prjXml.Project.GetAttribute("xmlns"));
	$startActionElement = $prjXml.CreateElement("StartAction", $prjXml.Project.GetAttribute("xmlns"));
	$propertyGroupElement.AppendChild($startActionElement) | Out-Null
	$propertyGroupElement.StartAction = "Program"
	$startProgramElement = $prjXml.CreateElement("StartProgram", $prjXml.Project.GetAttribute("xmlns"));
	$propertyGroupElement.AppendChild($startProgramElement) | Out-Null
	$propertyGroupElement.StartProgram = "`$(ProjectDir)`$(OutputPath)MassTransit.Host.RabbitMQ.exe"
	$prjXml.project.AppendChild($propertyGroupElement) | Out-Null
	$writerSettings = new-object System.Xml.XmlWriterSettings
	$writerSettings.OmitXmlDeclaration = $false
	$writerSettings.NewLineOnAttributes = $false
	$writerSettings.Indent = $true
	$projectFilePath = Resolve-Path -Path $project.FullName
	$writer = [System.Xml.XmlWriter]::Create($projectFilePath, $writerSettings)
	$prjXml.WriteTo($writer)
	$writer.Flush()
	$writer.Close()
}

function Add-HostConfigToOutputDirectory {
	$configFile = $project.ProjectItems.Item("MassTransit.Host.RabbitMQ.exe.config")

	$copyToOutput1 = $configFile.Properties.Item("CopyToOutputDirectory")
	$copyToOutput1.Value = 1
}
$project.Save()
Add-HostConfigToOutputDirectory
$project.Save()
Write-Host "Adding startup project..." -BackgroundColor yellow -foregroundcolor black
Add-StartServiceHostProject

Write-Host "Done!" -BackgroundColor yellow -foregroundcolor black