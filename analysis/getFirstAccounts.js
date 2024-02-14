#!/usr/local/bin/node

// Finds the first month in which a user was registered for each mastodon instances given
// like in sort.js, this shows an amount per month but by taking the first registered user, it shows instance creation

const fs = require("fs");

let res = {};
fs.readdirSync(".").forEach(file => {
	if (file.endsWith(".json")) {
		let first;
		let firstdate = Number.MAX_VALUE;
		let firstdateStr;
		JSON.parse(fs.readFileSync(file, "utf8")).forEach(elem => {
			// take only the year and month from a date string and convert it into a number which can be compared
			// e.g. 2023-08-16 -> 202308
			let dateNum = Number(elem.created_at.substring(0, 7).replace("-", ""));
			if (dateNum < firstdate) {
				firstdate = dateNum;
				first = elem;
				firstdateStr = elem.created_at.substring(0, 7);
			}
		});
		if (res[firstdateStr] === undefined)
			res[firstdateStr] = 1;
		else
			res[firstdateStr]++;
	}
});
fs.writeFileSync("instance_creations.json", JSON.stringify(res, null, 4));
