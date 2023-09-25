import 'dart:convert';
import 'dart:typed_data';

import 'package:encrypt/encrypt.dart' as encrypt;
import 'package:nobody/references.dart';

class Store<T> {
  final String storeName;
  final Future<Directory> Function() getDirectory;
  final Function(Uint8List) deserialize;
  final Function(T) serialize;

  Store({
    required this.storeName,
    this.getDirectory = _getDocumentDirectory,
    required this.deserialize,
    required this.serialize,
  });

  Future<String> get _localPath async {
    final directory = await getDirectory();
    return directory.path;
  }

  Future<Directory> get _localDirectory async {
    final path = await _localPath;
    return Directory('$path/$storeName')..createSync(recursive: true);
  }

  static Future<Directory> _getDocumentDirectory() async {
    final directory = await Directory.current;
    return directory;
  }

  Future<void> set(String itemName, T value) async {
    final directory = await _localDirectory;

    final file = File('${directory.path}/$itemName');

    final bytes = serialize(value);
    await file.writeAsBytes(bytes, flush: true);
  }

  Future<T?> get(String itemName) async {
    try {
      final directory = await _localDirectory;
      final file = File('${directory.path}/$itemName');
      if (!file.existsSync()) {
        return null;
      }
      final bytes = await file.readAsBytes();
      final data = deserialize(bytes);
      return data as T?;
    } catch (e) {
      print("Error loading data: $e");
      return null;
    }
  }
}

const key = 'your_key_here';

String aesEncrypt(String plainText, String key) {
  final keyBytes = utf8.encode(key);
  final paddedKey = Uint8List.fromList(List<int>.from(keyBytes)
    ..addAll(List<int>.filled(32 - keyBytes.length, 0)));
  final encrypterKey = encrypt.Key(paddedKey);
  final iv = encrypt.IV(Uint8List.fromList(List<int>.filled(16, 0)));
  final encrypter = encrypt.Encrypter(encrypt.AES(encrypterKey));

  final encrypted = encrypter.encrypt(plainText, iv: iv);
  return base64.encode(encrypted.bytes);
}

String aesDecrypt(String encryptedText, String key) {
  final keyBytes = utf8.encode(key);
  final paddedKey = Uint8List.fromList(List<int>.from(keyBytes)
    ..addAll(List<int>.filled(32 - keyBytes.length, 0)));
  final encrypterKey = encrypt.Key(paddedKey);
  final iv = encrypt.IV(Uint8List.fromList(List<int>.filled(16, 0)));
  final encrypter = encrypt.Encrypter(encrypt.AES(encrypterKey));

  final decrypted = encrypter
      .decrypt(encrypt.Encrypted(base64.decode(encryptedText)), iv: iv);
  return decrypted;
}

final passwordStore = Store<String>(
  storeName: 'passwords',
  deserialize: (bytes) => aesDecrypt(base64.encode(bytes), key),
  serialize: (value) => base64.decode(aesEncrypt(value, key)),
);

final tokenStore = Store<String>(
  storeName: 'tokens',
  deserialize: (bytes) => aesDecrypt(base64.encode(bytes), key),
  serialize: (value) => base64.decode(aesEncrypt(value, key)),
);

final dateStore = Store<DateTime>(
  storeName: 'dates',
  deserialize: (bytes) => DateTime.fromMillisecondsSinceEpoch(
      int.parse(String.fromCharCodes(bytes))),
  serialize: (value) => value.millisecondsSinceEpoch.toString().codeUnits,
);
