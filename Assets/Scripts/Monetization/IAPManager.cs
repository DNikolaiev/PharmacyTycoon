using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using System.Linq;
public class IAPManager : MonoBehaviour, IStoreListener
{

    private static IStoreController m_StoreController;          // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

    // Product identifiers for all products capable of being purchased: 
    // "convenience" general identifiers for use with Purchasing, and their store-specific identifier 
    // counterparts for use with and outside of Unity Purchasing. Define store-specific identifiers 
    // also on each platform's publisher dashboard (iTunes Connect, Google Play Developer Console, etc.)

    // General product identifiers for the consumable, non-consumable, and subscription products.
    // Use these handles in the code to reference which product to purchase. Also use these values 
    // when defining the Product Identifiers on the store. Except, for illustration purposes, the 
    // kProductIDSubscription - it has custom Apple and Google identifiers. We declare their store-
    // specific mapping to Unity Purchasing's AddProduct, below.
    public static string PRODUCT_CHEST_SIMPLE = "resources_simple";
    public static string PRODUCT_CHEST_ELITE = "resources_elite";
    public static string PRODUCT_COINS_SMALL = "medcoins_small";
    public static string PRODUCT_COINS_LARGE = "medcoins_large";
    public static string PRODUCT_MEDKIT = "medkit";
    public static string PRODUCT_UPGRADE = "production_upgrade";
    public static string PRODUCT_SECOND_CHANCE = "second_chance";
    public static string PRODUCT_AUTOCLICKER = "auto_clicker";

   

    void Start()
    {
        // If we haven't set up the Unity Purchasing reference
        if (m_StoreController == null)
        {
            // Begin to configure our connection to Purchasing
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        // If we have already connected to Purchasing ...
        if (IsInitialized())
        {
            // ... we are done here.
            return;
        }

        // Create a builder, first passing in a suite of Unity provided stores.
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(PRODUCT_CHEST_SIMPLE, ProductType.Consumable);
        builder.AddProduct(PRODUCT_CHEST_ELITE, ProductType.Consumable);
        builder.AddProduct(PRODUCT_COINS_SMALL, ProductType.Consumable);
        builder.AddProduct(PRODUCT_COINS_LARGE, ProductType.Consumable);
        builder.AddProduct(PRODUCT_AUTOCLICKER, ProductType.NonConsumable);
        builder.AddProduct(PRODUCT_MEDKIT, ProductType.Consumable);
        builder.AddProduct(PRODUCT_SECOND_CHANCE, ProductType.Consumable);
        builder.AddProduct(PRODUCT_UPGRADE, ProductType.Consumable);

        // Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
        // and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
        UnityPurchasing.Initialize(this, builder);
    }


    private bool IsInitialized()
    {
        // Only say we are initialized if both the Purchasing references are set.
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }


    public void BuyChestSimple()
    {
       
        BuyProductID(PRODUCT_CHEST_SIMPLE);
    }
    public void BuyChestElite()
    {

        BuyProductID(PRODUCT_CHEST_ELITE);
    }
    public void BuyMedCoinsSmall()
    {

        BuyProductID(PRODUCT_COINS_SMALL);
    }
    public void BuyCoinsLarge()
    {

        BuyProductID(PRODUCT_COINS_LARGE);
    }
    public void BuyUpgrade()
    {

        BuyProductID(PRODUCT_UPGRADE);
    }
    public void BuySecondChance()
    {

        BuyProductID(PRODUCT_SECOND_CHANCE);
    }
    public void BuyMedkit()
    {

        BuyProductID(PRODUCT_MEDKIT);
    }
    public void BuyAutoClicker()
    {

        BuyProductID(PRODUCT_AUTOCLICKER);
    }



    private void ShowMessage(string text)
    {
        if (GameController.instance == null) return;
        var messageBox = GameController.instance.buttons.messageBox;
        messageBox.Show(text);
    }

    void BuyProductID(string productId)
    {
        // If Purchasing has been initialized ...
        if (IsInitialized())
        {
            // ... look up the Product reference with the general product identifier and the Purchasing 
            // system's products collection.
            Product product = m_StoreController.products.WithID(productId);

            // If the look up found a product for this device's store and that product is ready to be sold ... 
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                // asynchronously.
                m_StoreController.InitiatePurchase(product);
            }
            // Otherwise ...
            else
            {
                // ... report the product look-up failure situation  
                ShowMessage(productId +" FAIL. Product either is not found or is not available for purchase");
            }
        }
        // Otherwise ...
        else
        {
            // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
            // retrying initiailization.
            ShowMessage("Not initialized");
        }
    }


    //  
    // --- IStoreListener
    //

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        // Purchasing has succeeded initializing. Collect our Purchasing references.
        Debug.Log("OnInitialized: PASS");

        // Overall Purchasing system, configured with products for this application.
        m_StoreController = controller;
        // Store specific subsystem, for accessing device-specific store features.
        m_StoreExtensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
        ShowMessage("Purchasing set-up failed: " + error.ToString());
    }


    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        // A consumable product has been purchased by this user.
        if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_CHEST_SIMPLE, StringComparison.Ordinal))
        {
            EventManager.TriggerEvent("OnDollarsPurchase", PRODUCT_CHEST_SIMPLE);

        }
        // Or ... a non-consumable product has been purchased by this user.
        else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_CHEST_ELITE, StringComparison.Ordinal))
        {
            EventManager.TriggerEvent("OnDollarsPurchase", PRODUCT_CHEST_ELITE);
        }
        // Or ... a subscription product has been purchased by this user.
        else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_COINS_SMALL, StringComparison.Ordinal))
        {
            EventManager.TriggerEvent("OnDollarsPurchase", PRODUCT_COINS_SMALL);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_COINS_LARGE, StringComparison.Ordinal))
        {
            EventManager.TriggerEvent("OnDollarsPurchase", PRODUCT_COINS_LARGE);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_MEDKIT, StringComparison.Ordinal))
        {
            EventManager.TriggerEvent("OnDollarsPurchase", PRODUCT_MEDKIT);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_UPGRADE, StringComparison.Ordinal))
        {
            EventManager.TriggerEvent("OnDollarsPurchase", PRODUCT_UPGRADE);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_SECOND_CHANCE, StringComparison.Ordinal))
        {
            EventManager.TriggerEvent("OnDollarsPurchase", PRODUCT_SECOND_CHANCE);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_AUTOCLICKER, StringComparison.Ordinal))
        {
            EventManager.TriggerEvent("OnDollarsPurchase", PRODUCT_AUTOCLICKER);
        }
        // Or ... an unknown product has been purchased by this user. Fill in additional products here....
        else
        {
            ShowMessage(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }

        // Return a flag indicating whether this product has completely been received, or if the application needs 
        // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
        // saving purchased products to the cloud, and when that save is delayed. 
        return PurchaseProcessingResult.Complete;
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
        // this reason with the user to guide their troubleshooting actions.
        ShowMessage(string.Format("FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }
}


