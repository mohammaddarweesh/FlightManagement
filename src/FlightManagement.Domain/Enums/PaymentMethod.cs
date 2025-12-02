namespace FlightManagement.Domain.Enums;

/// <summary>
/// Represents the payment method used for a transaction.
/// </summary>
public enum PaymentMethod
{
    /// <summary>
    /// Credit card payment.
    /// </summary>
    CreditCard = 0,

    /// <summary>
    /// Debit card payment.
    /// </summary>
    DebitCard = 1,

    /// <summary>
    /// PayPal payment.
    /// </summary>
    PayPal = 2,

    /// <summary>
    /// Bank transfer.
    /// </summary>
    BankTransfer = 3,

    /// <summary>
    /// Digital wallet (Apple Pay, Google Pay).
    /// </summary>
    DigitalWallet = 4,

    /// <summary>
    /// Cryptocurrency payment.
    /// </summary>
    Crypto = 5,

    /// <summary>
    /// Account credit/voucher.
    /// </summary>
    AccountCredit = 6
}

