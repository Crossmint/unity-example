# unity-example
Setup a Unity demonstration utilizing Crossmint in just a few minutes!

This example showcases Crossmint's Custodial Wallets and Minting ability.

This isn't a game but rather just a simple menu to demonstrate the ability you have using Crossmint in your own personal game/project.

## 1. Grab your Crossmint keys
We have two environments, one for testing and one for release. These environments are selected based on the subdomain you select. `staging.crossmint.com` is the testing environment and `www.crossmint.com` is the release environment. Throughout this tutorial, we will be using the staging subdomain (testing environment). These steps are identical for either environments with the only difference being the domain you're using.

### In the Crossmint console

1. Go to [staging.crossmint.com/console](https://staging.crossmint.com/console) and follow the steps to create an account.

2. Navigate to [API keys](https://staging.crossmint.com/console/projects/apiKeys) and click on New API Key. Then check the `nfts.mint`, `wallets.read`, `wallets.create` and `wallets:nfts.read` scopes -- this will give your API key permission to create and manage crypto wallets and mint for your users. Finally save your new key and copy the `CLIENT SECRET` and `Project ID` values for later.

### On your machine

Open the `BackendServer` project, rename the `.env.template` file to `.env`.

In the `.env` file, fill in your the fields with your information gathered from the Crossmint console (`CROSSMINT_X_CLIENT_SECRET` and `CROSSMINT_X_PROJECT_ID`).

The result should look something like this:

```bash
CROSSMINT_BASEURL=https://staging.crossmint.com
CROSSMINT_X_CLIENT_SECRET=YOUR_CROSSMINT_CLIENT_SECRET_HERE
CROSSMINT_X_PROJECT_ID=YOUR_CROSSMINT_PROJECT_ID_HERE
CROSSMINT_CHAIN="polygon"
CROSSMINT_COLLECTION_ID="default-polygon"
PORT=3000
```

You can now modify the chain, collection and port to your desired values.

And then save.

Please note: make sure the clent secret doesn't get leaked, as it would allow others to create wallets for your users.

## 2. Configure the BackendServer project

1. Open `index.js` and configure the project based on the instructions presented in the authentication method on line 9.

2. Start the server by running the following command in the `BackendServer` directory:

```bash
node index.js
```

## 3. Configure Unity

1. Open the project in Unity Editor (the project was created on 2021.3.21f1)

2. Open the C# project (Assets -> Open C# Project)

3. Open `Config.cs` and update the `BackendServer` url.

4. Ensure the `BackendServer` is running and start the game!
