import fetch from "node-fetch";

export async function createWallet(userId) {
    const url = `${process.env.CROSSMINT_BASEURL}/api/v1-alpha1/wallets`;
    const options = {
        method: "POST",
        body: JSON.stringify({ chain: process.env.CROSSMINT_CHAIN, userId: userId }),
    };

    return await fetchJSON(url, options);
}

export async function findExistingWallets(userId) {
    const url = `${process.env.CROSSMINT_BASEURL}/api/v1-alpha1/wallets?userId=${encodeURIComponent(userId)}`;
    const options = {
        method: "GET"
    };

    return await fetchJSON(url, options);
}

export async function mintToWallet(collectionId, chain, address) {
    const url = `${process.env.CROSSMINT_BASEURL}/api/2022-06-09/collections/${encodeURIComponent(collectionId)}/nfts`;
    const options = {
        method: 'POST',
        body: JSON.stringify({
            recipient: `${chain}:${address}`,
            metadata: {
                name: 'My first Mint API NFT',
                image: 'https://www.crossmint.com/assets/crossmint/logo.png',
                description: 'My NFT created via the mint API!'
            }
        })
    };

    return await fetchJSON(url, options);
}

export async function fetchMintsFromWallet(chain, address, page) {
    const url = `${process.env.CROSSMINT_BASEURL}/api/v1-alpha1/wallets/${chain}:${address}/nfts?page=${page}&perPage=20`;
    const options = {
        method: 'GET'
    };

    return await fetchJSON(url, options);
}

export async function fetchMintStatus(collectionId, id) {
    const url = `${process.env.CROSSMINT_BASEURL}/api/2022-06-09/collections/${encodeURIComponent(collectionId)}/nfts/${id}`;
    const options = {
        method: 'GET'
    };

    return await fetchJSON(url, options);
}

async function fetchJSON(url, options) {
    options.headers = {
        accept: "application/json",
        "X-CLIENT-SECRET": process.env.CROSSMINT_X_CLIENT_SECRET,
        "X-PROJECT-ID": process.env.CROSSMINT_X_PROJECT_ID
    };

    if (options.method === "POST") {
        options.headers["Content-Type"] = "application/json"
    }

    try {
        const response = await fetch(url, options);
        return await response.json();
    }
    catch (error) {
        console.log(error.message);
        return { error: true, message: "An internal error has occurred" };
    }
}