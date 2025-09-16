import 'package:edu_system_mobile/models/user_model.dart';
import 'package:edu_system_mobile/services/api_service.dart';
import 'package:get/get.dart';

class HomeController extends GetxController {
  var users = <User>[].obs;
  var isLoading = true.obs;

  final ApiService apiService = ApiService();

  @override
  void onInit() {
    fetchUsers();
    super.onInit();
  }

  void fetchUsers() async {
    try {
      isLoading(true);
      users.value = await apiService.getUsers();
    } finally {
      isLoading(false);
    }
  }
}
