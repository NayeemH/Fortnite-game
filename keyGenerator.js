const crypto = require('crypto');

const accessKey = crypto.randomBytes(64).toString('hex');
const refreshKey = crypto.randomBytes(64).toString('hex');