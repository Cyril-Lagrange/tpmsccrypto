
# Check TPM 

On window, click Start menu and type tpm.msc

Make sure Status says TPM is ready for use

# Create a virtual smart card:

Reference on TMP virtual smart card is available [here](https://docs.microsoft.com/en-us/windows/security/identity-protection/virtual-smart-cards/virtual-smart-card-use-virtual-smart-cards)

To create a TPM virtual smart card use the following command

    tpmvscmgr.exe create /name myVSC /pin prompt /adminkey random /generate

When prompted enter a PIN (8 char min)

# Instal certificate on smart card
Create a text file "cert.req" with following containt:

    [NewRequest]
    Subject = "CN=DELFI"
    KeyLength = 2048
    ProviderName = "Microsoft Smart Card Key Storage Provider"
    KeySpec = "AT_SIGNATURE"
    KeyUsage = "CERT_KEY_ENCIPHERMENT_KEY_USAGE"
    KeyUsageProperty = "NCRYPT_ALLOW_DECRYPT_FLAG"
    RequestType = Cert
    SMIME = FALSE
    [EnhancedKeyUsageExtension]
    OID=1.3.6.1.4.1.311.67.1.1

Run the following command:

    certreq -new cert.req

# Using sccrypto
scrypto comes with a builtin help, please check the help
