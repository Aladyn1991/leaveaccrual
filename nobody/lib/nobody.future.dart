// GENERATED CODE - DO NOT MODIFY BY HAND

// **************************************************************************
// FutureGenerator
// **************************************************************************

/// CODE GENERATED BY nomo_code GENERATOR. DO NOT EDIT.

/// IMPORTING ORIGINAL SOURCE
import 'nobody.dart';

/// GENERATED EXTENSION
extension ExOnline on Future<Online> {
  Future<Online> visit(
    String url,
  ) async {
    var Online = await this;
    return Online.visit(
      url,
    );
  }

  Future<Online> type(
    String selector,
    String text,
  ) async {
    var Online = await this;
    return Online.type(
      selector,
      text,
    );
  }

  Future<Online> click(
    String selector,
  ) async {
    var Online = await this;
    return Online.click(
      selector,
    );
  }

  Future<Online> waitFor(
    String selector,
  ) async {
    var Online = await this;
    return Online.waitFor(
      selector,
    );
  }

  Future<Online> has(
    String selector,
    String text,
  ) async {
    var Online = await this;
    return Online.has(
      selector,
      text,
    );
  }

  Future<Online> close() async {
    var Online = await this;
    return Online.close();
  }
}
