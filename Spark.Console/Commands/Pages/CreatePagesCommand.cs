using Spark.Console.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Spark.Console.Commands.Pages;

public class CreatePagesCommand
{
    private readonly static string PagePath = $"./Pages";

    public void Execute(string modelName)
    {
        string appName = UserApp.GetAppName();

        ConsoleOutput.GenerateAlert(new List<string>() { $"Creating pages for {modelName}" });

        bool wasGenerated = CreateIndexFiles(appName, modelName);

        if (!wasGenerated)
        {
            ConsoleOutput.WarningAlert(new List<string>() { $"{PagePath}/{modelName}s/Index.razor already exists. Nothing done." });
            return;
        }
        else
        {
            ConsoleOutput.SuccessAlert(new List<string>() { $"{PagePath}/{modelName}s/Index.razor and {PagePath}/{modelName}s/Index.razor.cs generated!" });
        }

        wasGenerated = CreateCreateFiles(appName, modelName);

        if (!wasGenerated)
        {
            ConsoleOutput.WarningAlert(new List<string>() { $"{PagePath}/{modelName}s/Create.razor already exists. Nothing done." });
            return;
        }
        else
        {
            ConsoleOutput.SuccessAlert(new List<string>() { $"{PagePath}/{modelName}s/Create.razor and {PagePath}/{modelName}s/Create.razor.cs generated!" });
        }

        wasGenerated = CreateShowFiles(appName, modelName);

        if (!wasGenerated)
        {
            ConsoleOutput.WarningAlert(new List<string>() { $"{PagePath}/{modelName}s/Show.razor already exists. Nothing done." });
            return;
        }
        else
        {
            ConsoleOutput.SuccessAlert(new List<string>() { $"{PagePath}/{modelName}s/Show.razor and {PagePath}/{modelName}s/Show.razor.cs generated!" });
        }

        wasGenerated = CreateEditFiles(appName, modelName);

        if (!wasGenerated)
        {
            ConsoleOutput.WarningAlert(new List<string>() { $"{PagePath}/{modelName}s/Edit.razor already exists. Nothing done." });
            return;
        }
        else
        {
            ConsoleOutput.SuccessAlert(new List<string>() { $"{PagePath}/{modelName}s/Edit.razor and {PagePath}/{modelName}s/Edit.razor.cs generated!" });
        }

        wasGenerated = CreateMapper(appName, modelName);

        if (!wasGenerated)
        {
            ConsoleOutput.WarningAlert(new List<string>() { $"{PagePath}/{modelName}s/Mapper.cs already exists." });
            return;
        }
        else
        {
            ConsoleOutput.SuccessAlert(new List<string>() { $"{PagePath}/{modelName}s/Mapper.cs generated!" });
        }
        
    }

    private bool CreateIndexFiles(string appName, string modelName)
    {
        var pluralModel = modelName + "s";
        var pageKebab = pluralModel.PascalToKebabCase();
        var lowerPluralModel = char.ToLower(pluralModel[0]) + pluralModel.Substring(1);
        var lowerSingularModel = char.ToLower(modelName[0]) + modelName.Substring(1);
        string content = $@"@page ""/{pageKebab}""

<PageTitle>{pluralModel}</PageTitle>

<h1 class=""font-bold text-xl"">All {pluralModel}</h1>

<a href=""/{pageKebab}/create"" class=""text-blue-500 hover:underline"">Create →</a>
<ul class=""space-y-2 list-disc list-inside mt-6"">
    @foreach (var {lowerSingularModel} in Model)
    {{
        <li>
            (<a class=""text-blue-500 hover:underline"" href=""/{pageKebab}/@{lowerSingularModel}.Id"">View</a> · <a class=""text-blue-500 hover:underline"" href=""/{pageKebab}/@{lowerSingularModel}.Id/edit"">Edit</a>)
        </li>
    }}
</ul>
";
        var success = Files.WriteFileIfNotCreatedYet($"{PagePath}/{pluralModel}", "Index.razor", content);

        if (!success) return false;
        var indexModelName = $"{modelName}IndexModel";
        string codeBehindContent = $@"using {appName}.Application.Database;
using {appName}.Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components;
using AutoMapper;

namespace {appName}.Pages.{pluralModel};

public partial class Index
{{

	public List<{indexModelName}> Model;

    [Inject]
    IDbContextFactory<DatabaseContext> Factory {{ get; set; }} = default!;

    [Inject]
	IMapper mapper {{ get; set; }} = default!;

	protected override void OnInitialized()
	{{
        using var db = Factory.CreateDbContext();
		var {lowerPluralModel} = db.{modelName}s.ToList();
		Model = {lowerPluralModel}
			.Select(i => mapper.Map<{indexModelName}>(i))
			.ToList();
	}}

}}

public class {indexModelName}
{{
	public int Id {{ get; set; }}
    // todo
}}
";
        return Files.WriteFileIfNotCreatedYet($"{PagePath}/{modelName}s", "Index.razor.cs", codeBehindContent);
    }


    private bool CreateCreateFiles(string appName, string modelName)
    {
        var pluralModel = modelName + "s";
        var pageKebab = pluralModel.PascalToKebabCase();
        var lowerPluralModel = char.ToLower(pluralModel[0]) + pluralModel.Substring(1);
        var lowerSingularModel = char.ToLower(modelName[0]) + modelName.Substring(1);
        string content = $@"@page ""/{pageKebab}/create""

<PageTitle>Create {modelName}</PageTitle>

<h1 class=""font-bold text-xl"">Create {modelName}</h1>

<EditForm Model=""Model"" OnValidSubmit=""Store"" class=""mt-4 space-y-4 max-w-sm"">
	<DataAnnotationsValidator />
	<div class=""flex justify-between"">
		<a href=""/{pageKebab}"" class=""px-2 py-1 text-black bg-gray-300"">Cancel</a>
		<button type=""submit"" class=""px-2 py-1 text-white bg-blue-500"">Save</button>
	</div>
</EditForm>
";
        var success = Files.WriteFileIfNotCreatedYet($"{PagePath}/{pluralModel}", "Create.razor", content);

        if (!success) return false;
        var createModelName = $"{modelName}CreateModel";
        string codeBehindContent = $@"using {appName}.Application.Database;
using {appName}.Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components;
using AutoMapper;
using Spark.Library.Extensions;
using System.ComponentModel.DataAnnotations;

namespace {appName}.Pages.{pluralModel};

public partial class Create
{{

    public {createModelName} Model {{ get; set; }} = new();

    [Inject]
    IDbContextFactory<DatabaseContext> Factory {{ get; set; }} = default!;

    [Inject]
	IMapper mapper {{ get; set; }} = default!;

    [Inject]
    NavigationManager NavigationManager {{ get; set; }} = default!;

	protected override void OnInitialized()
	{{
	}}

    public void Store()
    {{
        using var db = Factory.CreateDbContext();
        var {lowerSingularModel} = new {modelName}();
        mapper.Map(Model, {lowerSingularModel});
        db.{pluralModel}.Save({lowerSingularModel});
        NavigationManager.NavigateTo(""/{pageKebab}"");
    }}

}}

public class {createModelName}
{{
    // todo
}}
";
        return Files.WriteFileIfNotCreatedYet($"{PagePath}/{modelName}s", "Create.razor.cs", codeBehindContent);
    }

    private bool CreateEditFiles(string appName, string modelName)
    {
        var pluralModel = modelName + "s";
        var pageKebab = pluralModel.PascalToKebabCase();
        var lowerPluralModel = char.ToLower(pluralModel[0]) + pluralModel.Substring(1);
        var lowerSingularModel = char.ToLower(modelName[0]) + modelName.Substring(1);
        string content = $@"@page ""/{pageKebab}/{{id:int}}/edit""

<PageTitle>Edit {modelName} @Model.Id</PageTitle>

<h1 class=""font-bold text-xl"">Edit {modelName} @Model.Id</h1>

<EditForm Model=""Model"" OnValidSubmit=""Update"" class=""mt-4 space-y-4 max-w-sm"">
	<DataAnnotationsValidator />
	<div class=""flex justify-between"">
		<a href=""/{pageKebab}"" class=""px-2 py-1 text-black bg-gray-300"">Cancel</a>
		<button type=""submit"" class=""px-2 py-1 text-white bg-blue-500"">Update</button>
	</div>
</EditForm>
";
        var success = Files.WriteFileIfNotCreatedYet($"{PagePath}/{pluralModel}", "Edit.razor", content);

        if (!success) return false;
        var editModelName = $"{modelName}EditModel";
        string codeBehindContent = $@"using {appName}.Application.Database;
using {appName}.Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components;
using AutoMapper;
using Spark.Library.Extensions;
using System.ComponentModel.DataAnnotations;

namespace {appName}.Pages.{pluralModel};

public partial class Edit
{{
    [Parameter]
    public int Id {{ get; set; }}

    public {editModelName} Model {{ get; set; }} = new();

    [Inject]
    IDbContextFactory<DatabaseContext> Factory {{ get; set; }} = default!;

    [Inject]
	IMapper mapper {{ get; set; }} = default!;

    [Inject]
    NavigationManager NavigationManager {{ get; set; }} = default!;

	protected override void OnInitialized()
	{{
        using var db = Factory.CreateDbContext();
        var {lowerSingularModel} = db.{pluralModel}.Find(Id);
        mapper.Map({lowerSingularModel}, Model);
	}}

    public void Update()
    {{
        using var db = Factory.CreateDbContext();
        var {lowerSingularModel} = db.{pluralModel}.Find(Model.Id);
        mapper.Map(Model, {lowerSingularModel});
        db.{pluralModel}.Save({lowerSingularModel});
        NavigationManager.NavigateTo(""/{pageKebab}"");
    }}

}}

public class {editModelName}
{{
    [Required]
    public int Id {{ get; set; }}
    // todo
}}
";
        return Files.WriteFileIfNotCreatedYet($"{PagePath}/{modelName}s", "Edit.razor.cs", codeBehindContent);
    }


    private bool CreateShowFiles(string appName, string modelName)
    {
        var pluralModel = modelName + "s";
        var pageKebab = pluralModel.PascalToKebabCase();
        var lowerPluralModel = char.ToLower(pluralModel[0]) + pluralModel.Substring(1);
        var lowerSingularModel = char.ToLower(modelName[0]) + modelName.Substring(1);
        string content = $@"@page ""/{pageKebab}/{{id:int}}""

<PageTitle>{modelName} @Model.Id</PageTitle>

<h1 class=""font-bold text-xl"">{modelName} @Model.Id</h1>
<a href=""/{pageKebab}"" class=""text-blue-500 hover:underline"">back to all →</a>

<button @onclick=""Delete"" class=""px-2 py-1 text-white bg-blue-500 mt-2"">Delete</button>
";
        var success = Files.WriteFileIfNotCreatedYet($"{PagePath}/{pluralModel}", "Show.razor", content);

        if (!success) return false;
        var showModelName = $"{modelName}ShowModel";
        string codeBehindContent = $@"using {appName}.Application.Database;
using {appName}.Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components;
using AutoMapper;
using Spark.Library.Extensions;
using System.ComponentModel.DataAnnotations;

namespace {appName}.Pages.{pluralModel};

public partial class Show
{{
    [Parameter]
    public int Id {{ get; set; }}

    public {showModelName} Model {{ get; set; }} = new();

    [Inject]
    IDbContextFactory<DatabaseContext> Factory {{ get; set; }} = default!;

    [Inject]
	IMapper mapper {{ get; set; }} = default!;

    [Inject]
    NavigationManager NavigationManager {{ get; set; }} = default!;

	protected override void OnInitialized()
	{{
        using var db = Factory.CreateDbContext();
        var {lowerSingularModel} = db.{pluralModel}.Find(Id);
        mapper.Map({lowerSingularModel}, Model);
	}}

    public void Delete()
    {{
        using var db = Factory.CreateDbContext();
        var {lowerSingularModel} = db.{pluralModel}.Find(Id);
        db.{pluralModel}.Delete({lowerSingularModel});
        NavigationManager.NavigateTo(""/{pageKebab}"");
    }}

}}

public class {showModelName}
{{
    public int Id {{ get; set; }}
    // todo
}}
";
        return Files.WriteFileIfNotCreatedYet($"{PagePath}/{modelName}s", "Show.razor.cs", codeBehindContent);
    }

    private bool CreateMapper(string appName, string modelName)
    {
        var pageKebab = modelName.PascalToKebabCase();
        var pluralModel = modelName + "s";
        var lowerPluralModel = char.ToLower(pluralModel[0]) + pluralModel.Substring(1);
        var indexModelName = $"{modelName}IndexModel";
        var createModelName = $"{modelName}CreateModel";
        var showModelName = $"{modelName}ShowModel";
        var editModelName = $"{modelName}EditModel";
        string content = $@"using AutoMapper;
using {appName}.Application.Models;


namespace {appName}.Pages.{pluralModel};

public class Mapper : AutoMapper.Profile
{{
	public Mapper()
	{{
		CreateMap<{modelName}, {indexModelName}>();
        CreateMap<{createModelName}, {modelName}>();
        CreateMap<{modelName}, {showModelName}>();
        CreateMap<{editModelName}, {modelName}>().ReverseMap();
    }}
}}

";
        return Files.WriteFileIfNotCreatedYet($"{PagePath}/{pluralModel}", "Mapper.cs", content);
    }
}
