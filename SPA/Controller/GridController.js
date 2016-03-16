var GridController = function ($scope, $uibModal,Api) {
    $scope.data = {
        lowstockdata: {
            totalitems: 0,
            currentPage: 1,
            itemsperpage: 10,
            data: []
        }
    };

    function GetData() {
        
        if ($scope.selectedLocation!=null){
            $scope.data.lowstockdata.data = [];
            var request = {
                LocationId: $scope.selectedLocation.StockLocationId,
                LocationName: $scope.selectedLocation.LocationName,
                PageNumber: $scope.data.lowstockdata.currentPage,
                EntriesPerPage: $scope.data.lowstockdata.itemsperpage
            };
            SetBusy($("#LowStockLevelGrid"));
            Api.PostApiCall("StockQuery", "GetLowStockLevel", request, function (event) {
                SetBusy($("#LowStockLevelGrid"), true);
                if (event.hasErrors == true) {
                    alert("Error Getting data: " + event.error);
                } else {
                    $scope.data.lowstockdata.data = event.result.rows;
                    $scope.data.lowstockdata.totalitems = event.result.TotalItems;
                }
            });
        }

    }

    $scope.pageChanged = function () {
        GetData();
    }


    $scope.$watch('selectedLocation', function () {
        $scope.data.lowstockdata.currentPage = 1;
        GetData();
    });


    $scope.openProduct = function (product) {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/SPA/Views/ViewProductWindow.html',
            controller: 'ViewProductController',
            size: "",
            resolve: {
                data: product
            }
        });

        modalInstance.result.then(function (selectedItem) {
            $scope.data.lowstockdata.selectedItem = selectedItem;
        }, function () {
            $log.info('Modal dismissed at: ' + new Date());
        });
    }

}

GridController.$inject = ['$scope','$uibModal','Api'];