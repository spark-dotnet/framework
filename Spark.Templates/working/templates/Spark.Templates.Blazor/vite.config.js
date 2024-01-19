import { defineConfig } from 'vite';
import appsettings from './appsettings.json';
import { spawn } from "child_process";
import fs from "fs";
import path from "path";
import * as process from "process";

// Get base folder for certificates.
const baseFolder =
    process.env.APPDATA !== undefined && process.env.APPDATA !== ''
        ? `${process.env.APPDATA}/ASP.NET/https`
        : `${process.env.HOME}/.aspnet/https`;

// Generate the certificate name using the NPM package name
const certificateName = process.env.npm_package_name;

// Define certificate filepath
const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
// Define key filepath
const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

// Export Vite configuration
export default defineConfig(async () => {
    // Ensure the certificate and key exist
    if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
        // Wait for the certificate to be generated
        await new Promise((resolve) => {
            spawn('dotnet', [
                'dev-certs',
                'https',
                '--export-path',
                certFilePath,
                '--format',
                'Pem',
                '--no-password',
            ], { stdio: 'inherit', })
                .on('exit', (code) => {
                    resolve();
                    if (code) {
                        process.exit(code);
                    }
                });
        });
    };

    // Define Vite configuration
    return {
        name: 'spark',
        plugins: [],
        build: {
            emptyOutDir: true,
            manifest: false,
            outDir: './wwwroot/build',
            rollupOptions: {
                input: [
                    'Assets/Js/app.js',
                    'Assets/Css/app.css'
                ],
                output: {
                    entryFileNames: `assets/[name].js`,
                    chunkFileNames: `assets/[name].js`,
                    assetFileNames: `assets/[name].[ext]`
                }
            },
        },
        server: {
            port: appsettings.Vite.Server.Port,
            strictPort: true,
            https: {
                cert: certFilePath,
                key: keyFilePath
            },
            hmr: {
                host: "localhost",
                clientPort: appsettings.Vite.Server.Port,
            }
        }
    }
});