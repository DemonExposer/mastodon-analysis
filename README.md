# mastodon-analysis

/api/v1/directory for account registration data. offset and limit for pagination

mastodon.com.tr <br/>
mastoturk.org <br/>
use mastodon.social for comparison <br/>
https://github.com/mapperfr/academics-on-mastodon?search=1#serverscommunities <br/>
<br/>
politics: <br/>
social.overheid.nl <br/>
eupolicy.social <br/>

## Retrieval process

User accounts from various Mastodon servers were taken using the Mastodon API endpoint /api/v1/directory.
Only local accounts were grabbed for the academic and Turkish dataset. The mastodon.social instance was used as a baseline to compare against. Since the point of this is to have as many accounts as possible and also getting just local accounts, seemed to ignore a large part of the users, mastodon.social was queried for non-local accounts as well.<br/>
<br/>
For the academic dataset, the servers listed on https://github.com/mapperfr/academics-on-mastodon?search=1#serverscommunities were used (only the ones which use HTTPS). For the Turkish dataset, mastodon.com.tr and mastoturk.org were used.

## Analysis process

The retrieved accounts' registration dates were taken and grouped by month. Like this, it can be seen in which month the most users joined and how it compares to other months.
With this information, it can be seen what events influenced the usage of Mastodon.

## Visualization process

The monthly data was visualized in a bar chart using the <a href="https://www.chartjs.org">chart.js</a> library
<br/>
<br/>
For the instance creation, I just took the first registered user, maybe it is wrong with these customized dates, but it seems fine, otherwise the server contact could be taken, but I fear that that might not always be the first user
