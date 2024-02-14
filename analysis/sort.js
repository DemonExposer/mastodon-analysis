#!/usr/local/bin/node
const fs = require("fs");

var res = {};
JSON.parse(fs.readFileSync("all_accounts.json", "utf8")).forEach(account => {
	const date = account.created_at.substring(0, 7);
	if (res[date] === undefined) {
		res[date] = 1;
	} else {
		res[date]++;
	}
});
fs.writeFileSync("months.json", JSON.stringify(res, null, 4));