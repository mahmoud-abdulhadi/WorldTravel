(function () {

    "use strict"; 


    angular.module("simpleControls", [])
        .directive("waitCursor", waitCursor);

    function waitCursor() {
        return {
           
           
            template: `<div>
                      <div class= "text-center" >

                        <i class="fa fa-circle-o-notch fa-spin"></i> Loading...
                    </div>
                    </div>`,
          

        }; 
         
    }

})();