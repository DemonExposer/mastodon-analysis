#!/usr/local/bin/node

// This program merges all the JSON files of the different mastodon instances into one large JSON file

const fs = require("fs");

var res = [];
fs.readdirSync(".").forEach(file => {
	if (file.endsWith(".json")) {
		JSON.parse(fs.readFileSync(file, "utf8")).forEach(elem => res.push(elem));
	}
});
fs.writeFileSync("all_accounts.json", JSON.stringify(res, null, 4));