const { spawn } = require('child_process');



    
    let cmd =  'dotnet';
    var process = spawn(cmd, ['bin/Debug/netcoreapp2.2/CoreTemplateStudio.Cli.dll']);


    process.stdin.write("sync -p C:\\Projects\\WebTemplateStudio\n");
    process.stdin.write("close\n");


    process.stdout.on('data', (data) => {
        console.log('ExecuteNpmInstall - stdout data: ' + data);
    });
    process.stdout.on('close', () => {
        console.log('ExecuteNpmInstall - stdout close:');
    });
    process.stdout.on('end', (data) =>  {
        console.log('ExecuteNpmInstall - stdout end: ' + data);
    });       

    process.stderr.on('data', (data) => {
        console.log('ExecuteNpmInstall - stderr data: ' + data);
    });
    process.stderr.on('close', (error) => {
        console.log('ExecuteNpmInstall - stderr close: ' + error);
    });  

    process.on('message', (data) => {
        console.log('ExecuteNpmInstall - message: ' + data);
    });
    process.on('disconnect', (data) => {
        console.log('ExecuteNpmInstall - disconnect: ' + data);
    });
    process.on('error', (code) => {
        console.log('ExecuteNpmInstall - error: ' + code);
    });
    process.on('exit', (code) => {
        console.log('ExecuteNpmInstall - exit: ' + code);

    });
    process.on('close', (code) =>  {
        console.log('ExecuteNpmInstall - close: ' + code);
    });