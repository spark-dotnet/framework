import { defineConfig } from 'vite';
import appsettings from './appsettings.json';

export default defineConfig({
    name: 'spark',
    plugins: [],
    build: {
        manifest: appsettings.Vite.Manifest,
        outDir: './wwwroot/build',
        rollupOptions: {
            input: [
                'Assets/Js/app.js',
                'Assets/Css/app.css'
            ],
        },
    },
    server: {
        port: appsettings.Vite.Server.Port,
        strictPort: true,
        hmr: {
            clientPort: appsettings.Vite.Server.Port,
        }
    }
});