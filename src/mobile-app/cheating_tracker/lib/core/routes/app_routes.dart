import 'package:flutter/material.dart';

abstract class AppRoutes {
  static const String initialRoute = 'initial';

  static Map<String, Widget Function(BuildContext)> router() {
    return {'initial': (context) => Placeholder()};
  }
}
