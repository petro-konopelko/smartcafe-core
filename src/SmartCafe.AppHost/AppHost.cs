var builder = DistributedApplication.CreateBuilder(args);

var postgresPassword = builder.AddParameter("postgres-password", secret: true);

// Add PostgreSQL
var postgres = builder
    .AddPostgres("postgres", password: postgresPassword)
    .WithDataVolume("menu-postgres-data")
    .WithPgAdmin()
    .AddDatabase("MenuDb");

// Add Azure Storage emulator (Azurite)
var storage = builder.AddAzureStorage("storage")
    .RunAsEmulator(c => c.WithBlobPort(10000));

var blobs = storage.AddBlobs("blobs");

// Add Menu Migrator
var migrator = builder.AddProject<Projects.SmartCafe_Menu_Migrator>("migrator")
    .WithReference(postgres)
    .WaitFor(postgres);

// Add Menu API
var menuApi = builder.AddProject<Projects.SmartCafe_Menu_API>("menu-api")
    .WaitForCompletion(migrator)
    .WaitFor(postgres)
    .WithReference(postgres)
    .WithReference(blobs);

// Add Angular Admin Client — starts last, after API is healthy.
builder.AddJavaScriptApp("admin", "../../../smartcafe-admin-client", "start")
    .WithReference(menuApi)
    .WaitFor(menuApi)
    .WithEnvironment("MENU_API_URL", menuApi.GetEndpoint("https"))
    .WithHttpEndpoint(port: 4200, isProxied: false)
    .WithExternalHttpEndpoints();

builder.Build().Run();
