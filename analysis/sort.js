#!/usr/local/bin/node

// This takes the account creation date for each account and puts it into monthly buckets
// This way it can be seen how many users joined the given collection of instances for every month

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