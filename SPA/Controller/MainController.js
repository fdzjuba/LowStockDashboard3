var MainController = function ($scope, Api) {
    $scope.models = {
        locations: [
                       
        ]
    };
    $scope.selectedLocation = null;

    $scope.changeLocation = function (loc) {
        $scope.selectedLocation = loc;
    }

    function GetLocations() {
        SetBusy($("#LocationSelector"));

        Api.GetApiCall("Locations", "GetLocations", function (event) {
            SetBusy($("#LocationSelector"),true);
            if (event.hasErrors == true) {
                $scope.setError(event.error);
            } else {
                $scope.models.locations = event.result;
                if ($scope.models.locations.length > 0) {
                    $scope.selectedLocation = $scope.models.locations[0];
                }
            }
        });
    }

    GetLocations();

}

MainController.$inject = ['$scope','Api'];