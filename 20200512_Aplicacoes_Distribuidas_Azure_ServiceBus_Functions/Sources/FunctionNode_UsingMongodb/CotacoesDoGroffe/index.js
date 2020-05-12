
module.exports = async function(context, acao) {
    context.log('JavaScript ServiceBus topic trigger function processed message', acao);

    const assert = require('assert');

    const MongoClient = require('mongodb').MongoClient;
    const ObjectId = require('mongodb').ObjectID;
    const url = process.env.demoazuresb_COSMOSDB;

    const docAcao = {
        codigo: acao.Codigo,
        valor: acao.Valor,
        date: new Date(),
        type: acao.Valor > 1000 ? 'high' : 'low'
    };

    MongoClient.connect(url, (err, client) => {
        assert.equal(err, null);
        const db = client.db('demosb');
        db.collection('acoes').insertOne(docAcao, (err, result) => {
            assert.equal(err, null);
            context.log("Inserted a document into the families collection.");
            client.close();
        })
    });
    
};