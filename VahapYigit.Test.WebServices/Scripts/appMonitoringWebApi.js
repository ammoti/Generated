var appMonitoringWebApi = angular.module('appMonitoringWebApi', ['ngRoute']);

appMonitoringWebApi.config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {
	$routeProvider.
		when('/LoggingSystem', {
			templateUrl: 'Views/Monitoring/LoggingSystem.html',
			controller: 'loggingSystemController'
		}).
		when('/CacheStatistics', {
			templateUrl: 'Views/Monitoring/CacheStatistics.html',
			controller: 'cacheStatisticsController'
		}).
		when('/ExecutionTracer', {
			templateUrl: 'Views/Monitoring/ExecutionTracer.html',
			controller: 'executionTracerController'
		}).
		when('/Tables', {
			templateUrl: 'Views/Monitoring/Tables.html',
			controller: 'tablesController'
		}).
		when('/ProcessLogs', {
			templateUrl: 'Views/Monitoring/ProcessLogs.html',
			controller: 'processLogsController'
		}).
		when('/ProcessErrorLogs', {
			templateUrl: 'Views/Monitoring/ProcessErrorLogs.html',
			controller: 'processErrorLogsController'
		}).
		otherwise({
			templateUrl: 'Views/Monitoring/LoggingSystem.html',
			controller: 'loggingSystemController'
		});

	//$locationProvider.html5Mode(true);
}]);

appMonitoringWebApi.controller('mainController', function ($scope) {
	
});

appMonitoringWebApi.controller('loggingSystemController', ['$scope', '$http', function ($scope, $http) {

	$scope.data = null;

	$scope.$evalAsync(function () {

		$http.get('/api/Monitoring/GetLogContent').
			success(function (data, status, headers, config) {
				$scope.data = data;
			}).
			error(function (data, status, headers, config) { });
	});
}]);

appMonitoringWebApi.controller('cacheStatisticsController', ['$scope', '$http', function ($scope, $http) {

	$scope.itemCount = 0;
	$scope.requestCount = 0;
	$scope.hitCount = 0;
	$scope.hitCountPercentage = 0;
	$scope.missCount = 0;
	$scope.missCountPercentage = 0;

	$scope.data = null;

	$scope.$evalAsync(function () {

		$http.get('/api/Monitoring/GetCacheStatistics').
			success(function (data, status, headers, config) {
				$scope.itemCount = data.ItemCount;
				$scope.requestCount = data.RequestCount;
				$scope.hitCount = data.HitCount;
				$scope.hitCountPercentage = data.HitCountPercentage;
				$scope.missCount = data.MissCount;
				$scope.missCountPercentage = data.MissCountPercentage;
			}).
			error(function (data, status, headers, config) { });

		$http.get('/api/Monitoring/GetCacheContent').
			success(function (data, status, headers, config) {
				$scope.data = data;
			}).
			error(function (data, status, headers, config) { });
	});
}]);

appMonitoringWebApi.controller('executionTracerController', ['$scope', '$http', function ($scope, $http) {

	$scope.data = null;

	$scope.$evalAsync(function () {

		$http.get('/api/Monitoring/GetExecutionTraces').
			success(function (data, status, headers, config) {
				$scope.data = data;
			}).
			error(function (data, status, headers, config) { });
	});
}]);

appMonitoringWebApi.controller('tablesController', ['$scope', '$http', function ($scope, $http) {

	$scope.$evalAsync(function () {

		$http.get('/api/Monitoring/GetTableInformation').
			success(function (data, status, headers, config) {
				$scope.data = data;
			}).
			error(function (data, status, headers, config) { });
	});
}]);

appMonitoringWebApi.controller('processLogsController', ['$scope', '$http', function ($scope, $http) {

	$scope.$evalAsync(function () {

		$http.get('/api/Monitoring/GetProcessLogs').
			success(function (data, status, headers, config) {
				$scope.data = data;
			}).
			error(function (data, status, headers, config) { });
	});
}]);

appMonitoringWebApi.controller('processErrorLogsController', ['$scope', '$http', function ($scope, $http) {

	$scope.$evalAsync(function () {

		$http.get('/api/Monitoring/GetProcessErrorLogs').
			success(function (data, status, headers, config) {
				$scope.data = data;
			}).
			error(function (data, status, headers, config) { });
	});
}]);