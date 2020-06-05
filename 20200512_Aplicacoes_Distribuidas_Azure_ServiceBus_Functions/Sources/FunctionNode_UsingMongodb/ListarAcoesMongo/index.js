module.exports = async function (context, req) {
    context.log('JavaScript HTTP trigger function processed a request.');

    const assert = require('assert');

    const MongoClient = require('mongodb').MongoClient;
    const ObjectId = require('mongodb').ObjectID;
    const url = process.env.demoazuresb_COSMOSDB;

    const client = await MongoClient.connect(url);
    const db = client.db('demosb');
    const result = await db.collection('acoes').find().toArray();
    context.res = {
        status: 200,
        body: result,
        headers: {
            'Content-Type': 'application/json'
        }
    };
    context.done();
};