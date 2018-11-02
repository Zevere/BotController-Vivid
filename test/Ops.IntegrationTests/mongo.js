const conn = new Mongo("mongo");
const db = conn.getDB("borzoo");

(function () {
    print('## ensuring db is initialized...')

    function wait(milliseconds) {
        const start = new Date()
        while (true)
            if (milliseconds <= (new Date() - start))
                return;
    }

    let times = 0;
    while (times < 5) {
        const collections = db.getCollectionNames()
        if (collections.length && db.users.count()) {
            print('## db is initialized and users collection has some documents')
            return;
        }
        wait(1000 * times)
        times++
    }
    throw `## ERROR: database is not initialized`
})();

function daysAgo(numberOfDays) {
    const now = new Date()
    return new Date(now.setDate(now.getDate() - numberOfDays))
}

print('## inserting new user "jsmith"...');
let result = db.users.insertOne({
    name: 'jsmith',
    pass: 'password',
    fname: 'John',
    token: '11TOKEN11',
    joined: daysAgo(5)
});


print('## Finished Successfully ✅');
