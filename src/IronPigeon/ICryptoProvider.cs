﻿namespace IronPigeon {
	/// <summary>
	/// Implements the cryptographic algorithms that protect users and data required by the IronPigeon protocol.
	/// </summary>
	public interface ICryptoProvider {
		/// <summary>
		/// Gets or sets the name of the hash algorithm to use.
		/// </summary>
		string HashAlgorithmName { get; set; }

		/// <summary>
		/// Gets or sets the configuration to use for symmetric encryption.
		/// </summary>
		EncryptionConfiguration SymmetricEncryptionConfiguration { get; set; }

		/// <summary>
		/// Gets or sets the size of the key used for symmetric blob encryption.
		/// </summary>
		int SymmetricEncryptionKeySize { get; set; }

		/// <summary>
		/// Gets or sets the size of the key used for asymmetric signatures.
		/// </summary>
		int SignatureAsymmetricKeySize { get; set; }

		/// <summary>
		/// Gets or sets the size of the key used for asymmetric encryption.
		/// </summary>
		int EncryptionAsymmetricKeySize { get; set; }

		/// <summary>
		/// Asymmetrically signs a data blob.
		/// </summary>
		/// <param name="data">The data to sign.</param>
		/// <param name="signingPrivateKey">The private key used to sign the data.</param>
		/// <returns>The signature.</returns>
		byte[] Sign(byte[] data, byte[] signingPrivateKey);

		/// <summary>
		/// Verifies the asymmetric signature of some data blob.
		/// </summary>
		/// <param name="signingPublicKey">The public key used to verify the signature.</param>
		/// <param name="data">The data that was signed.</param>
		/// <param name="signature">The signature.</param>
		/// <returns>A value indicating whether the signature is valid.</returns>
		bool VerifySignature(byte[] signingPublicKey, byte[] data, byte[] signature);

		/// <summary>
		/// Symmetrically encrypts the specified buffer using a randomly generated key.
		/// </summary>
		/// <param name="data">The data to encrypt.</param>
		/// <returns>The result of the encryption.</returns>
		SymmetricEncryptionResult Encrypt(byte[] data);

		/// <summary>
		/// Symmetrically decrypts a buffer using the specified key.
		/// </summary>
		/// <param name="data">The encrypted data and the key and IV used to encrypt it.</param>
		/// <returns>The decrypted buffer.</returns>
		byte[] Decrypt(SymmetricEncryptionResult data);

		/// <summary>
		/// Asymmetrically encrypts the specified buffer using the provided public key.
		/// </summary>
		/// <param name="encryptionPublicKey">The public key used to encrypt the buffer.</param>
		/// <param name="data">The buffer to encrypt.</param>
		/// <returns>The ciphertext.</returns>
		byte[] Encrypt(byte[] encryptionPublicKey, byte[] data);

		/// <summary>
		/// Asymmetrically decrypts the specified buffer using the provided private key.
		/// </summary>
		/// <param name="decryptionPrivateKey">The private key used to decrypt the buffer.</param>
		/// <param name="data">The buffer to decrypt.</param>
		/// <returns>The plaintext.</returns>
		byte[] Decrypt(byte[] decryptionPrivateKey, byte[] data);

		/// <summary>
		/// Computes the hash of the specified buffer.
		/// </summary>
		/// <param name="data">The data to hash.</param>
		/// <returns>The computed hash.</returns>
		byte[] Hash(byte[] data);

		/// <summary>
		/// Generates a key pair for asymmetric cryptography.
		/// </summary>
		/// <param name="keyPair">Receives the serialized key pair (includes private key).</param>
		/// <param name="publicKey">Receives the public key.</param>
		void GenerateSigningKeyPair(out byte[] keyPair, out byte[] publicKey);

		/// <summary>
		/// Generates a key pair for asymmetric cryptography.
		/// </summary>
		/// <param name="keyPair">Receives the serialized key pair (includes private key).</param>
		/// <param name="publicKey">Receives the public key.</param>
		void GenerateEncryptionKeyPair(out byte[] keyPair, out byte[] publicKey);
	}
}
