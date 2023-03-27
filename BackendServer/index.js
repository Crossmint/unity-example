import { createWallet, findExistingWallets, mintToWallet, fetchMintsFromWallet } from './wallet.js';
import { config } from "dotenv";
import express from 'express';

config();
const app = express();
const port = process.env.PORT;

app.use(express.json());

async function authenticate(req) {
    // This is where you would implement authentication via game center, play store, email, your own impl, etc...
    // You must implement this method because otherwise you're at risk of someone just spamming this endpoint and
    // draining your credits or potentially even minting something valuable to their own personal wallet.

    // As a safety precaution, we've excluded transfers of nfts as it poses an even bigger risk if authentication isn't
    // correctly implemented. As a result, we strongly urge you to avoid adding transfer in here and instead attempt to
    // prevent the need for having to use the transfer system altogether or ensure your code is thoroughly audited by
    // a security expert to ensure everything is up to code.
    throw new Error("User Authentication logic must be implemented in order to proceed.");
}

// Create
app.post('/api/wallet', async (req, res) => {
    const userId = req.body.userId;
    if (userId == null) {
        res.send("Missing `userId` parameter");
        return;
    }

    await authenticate(req);
    const response = await createWallet(userId);
    res.send(response);
});

// Fetch wallet
app.get('/api/wallet', async (req, res) => {
    const userId = req.query.userId;
    if (userId == null) {
        res.send("Missing `userid` parameter");
        return;
    }

    await authenticate(req);
    const response = await findExistingWallets(userId);
    res.send(response);
});

// Mint an NFT to a wallet
app.post('/api/mint', async (req, res) => {
    const collectionId = process.env.CROSSMINT_COLLECTION_ID;
    const chain = process.env.CROSSMINT_CHAIN;
    const address = req.body.address;
    if (address == null) {
        res.send("Missing one or more of the following parameters: `collectionId`, `chain`, and `address`");
        return;
    }

    await authenticate(req);
    const response = await mintToWallet(collectionId, chain, address);
    res.send(response);
});

// Fetch nfts
app.get('/api/mint', async (req, res) => {
    const chain = process.env.CROSSMINT_CHAIN;
    const address = req.query.address;
    if (address == null) {
        res.send("Missing one or more of the following parameters: `chain`, and `address`");
        return;
    }

    await authenticate(req);
    const page = req.query.page ?? 1;
    const response = await fetchMintsFromWallet(chain, address, page);
    res.send(response);
});

app.listen(port, () => {
    console.log(`Application is now listening on port: ${port}`);
});