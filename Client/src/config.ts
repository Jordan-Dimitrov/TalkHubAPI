import * as fs from 'fs';
export let config = JSON.parse(fs.readFileSync('./config.json', 'utf8'));
