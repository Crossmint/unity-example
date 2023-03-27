import fetch from "node-fetch";

export async function createWallet(userId) {
    const url = `${process.env.CROSSMINT_BASEURL}/api/v1-alpha1/wallets`;
    const options = {
        method: "POST",
        headers: {
            accept: "application/json",
            "content-type": "application/json",
            "X-CLIENT-SECRET": process.env.CROSSMINT_X_CLIENT_SECRET,
            "X-PROJECT-ID": process.env.CROSSMINT_X_PROJECT_ID,
        },
        body: JSON.stringify({ chain: process.env.CROSSMINT_CHAIN, userId: userId }),
    };

    try {
        const response = await fetch(url, options);
        return await response.json();
    }
    catch (error) {
        return { error: true, message: "An internal error has occurred." };
    }
}

export async function findExistingWallets(userId) {
    const url = `${process.env.CROSSMINT_BASEURL}/api/v1-alpha1/wallets?userId=${encodeURIComponent(userId)}`;
    const options = {
        method: "GET",
        headers: {
            accept: "application/json",
            "X-CLIENT-SECRET": process.env.CROSSMINT_X_CLIENT_SECRET,
            "X-PROJECT-ID": process.env.CROSSMINT_X_PROJECT_ID,
        },
    };

    try {
        const response = await fetch(url, options);
        return await response.json();
    }
    catch (error) {
        return { error: true, message: "An internal error has occurred" };
    }
}

export async function mintToWallet(collectionId, chain, address) {
    const url = `${process.env.CROSSMINT_BASEURL}/api/2022-06-09/collections/${encodeURIComponent(collectionId)}/nfts`;
    const options = {
        method: 'POST',
        headers: {
            'content-type': 'application/json',
            "X-CLIENT-SECRET": process.env.CROSSMINT_X_CLIENT_SECRET,
            "X-PROJECT-ID": process.env.CROSSMINT_X_PROJECT_ID,
        },
        body: JSON.stringify({
            recipient: `${chain}:${address}`,
            metadata: {
                name: 'My first Mint API NFT',
                image: 'https://www.crossmint.com/assets/crossmint/logo.png',
                description: 'My NFT created via the mint API!'
            }
        })
    };

    try {
        const response = await fetch(url, options);
        return await response.json();
    }
    catch (error) {
        return { error: true, message: "An internal error has occurred" };
    }
}

export async function fetchMintsFromWallet(chain, address, page) {
    const url = `${process.env.CROSSMINT_BASEURL}/api/v1-alpha1/wallets/${chain}:${address}/nfts?page=${page}&perPage=20`;
    const options = {
        method: 'GET',
        headers: {
            accept: 'application/json',
            "X-CLIENT-SECRET": process.env.CROSSMINT_X_CLIENT_SECRET,
            "X-PROJECT-ID": process.env.CROSSMINT_X_PROJECT_ID,
        }
    };

    try {
        const response = await fetch(url, options);
        return await response.json();
    }
    catch (error) {
        return { error: true, message: "An internal error has occurred" };
    }
}