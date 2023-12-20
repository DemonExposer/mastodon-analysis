const fs = require("fs");

let res = {};
fs.readdirSync(".").forEach(file => {
	if (file.endsWith(".json")) {
		let first;
		let firstdate = 30000000;
		let firstdateStr;
		JSON.parse(fs.readFileSync(file, "utf8")).forEach(elem => {
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
