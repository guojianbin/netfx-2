param($installPath, $toolsPath, $package, $project)

	$project.Object.References.Add("System.Data.Services.Client")
	$project.Object.References.Add("System.Data.Entity.Design")