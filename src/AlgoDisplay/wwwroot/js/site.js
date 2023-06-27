// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//function showImg(obj) {
//    var files = obj.files
//    // document.getElementById("imgContainer").innerHTML = getImgsByUrl(files)

//    getImgsByFileReader(document.getElementById("imgContainer"), files)
//}

// 使用window.URL.createObjectURL(file)读取file实例并显示图片
//function getImgsByUrl(files) {
//    var elements = ''
//    for (var i = 0; i < files.length; i++) {
//        var url = window.URL.createObjectURL(files[i])
//        elements += "<img src='" + url + "' style='width: 40px; height: 40px; vertical-align: middle; margin-right: 5px;' />"
//    }
//    return elements
//}

// 使用FileReader读取file实例并显示图片
//function getImgsByFileReader(el, files) {
//    for (var i = 0; i < files.length; i++) {
//        let img = document.createElement('img')
//        img.setAttribute('style', 'width:500px; vertical-align: middle; margin:20px;')
//        el.appendChild(img)
//        var reader = new FileReader()
//        reader.onload = function (e) {
//            img.src = e.target.result
//        }
//        reader.readAsDataURL(files[i])
//    }
//}

//上传单张图片
function showImg(input) {
    var file = input.files[0];
    var url = window.URL.createObjectURL(file)
    document.getElementById('upload').src = url;
}

// 获取选择算法的下拉菜单和算法介绍的 <div> 元素
// 获取算法选择器
const algorithmSelector = document.getElementById('algorithm');
const algorithmDescription = document.querySelector('.algorithm-description');
const form = document.querySelector('form');
const originalImageDiv = document.getElementById('upload');
const processImageDiv = document.getElementById('process');

// 获取所有包含图片的img元素
const originalImageElements = originalImageDiv.getElementsByTagName('img');

//点击运行按钮提示
function runAlgorithm() {
    var algorithm = document.getElementById("algorithm").value;
    var image = document.getElementById("image").value;
    var parameterContainer = document.getElementById("parameter-container");
    var parameters = parameterContainer.querySelectorAll("input");

    if (algorithm === "default") {
        alert("请选择算法！");
        return false;
    }

    if (image === "") {
        alert("请先上传图片！");
        return false;
    }

    //for (var i = 0; i < parameters.length; i++) {
    //    if (parameters[i].value === "") {
    //        alert("请输入完整参数！");
    //        return false;
    //    }
    //}

    // 如果所有条件都满足，可以进行算法运行
    // ...
}


//重置按钮
function resetForm() {
    form.reset();
    algorithmDescription.textContent = '请选择一个算法查看相应简介。'
    for (let i = 0; i < originalImageElements.length; i++) {
        originalImageElements[i].src = '';
    }
    originalImageDiv.src = "/icons/grey.jpg";
    processImageDiv.src = "/icons/grey.jpg";
    //重置参数选择
    parameterForm.innerHTML = `
                <div class="card-body bg-light d-flex align-items-center">
                        请选择算法以输入相应参数！
                </div>
     `;
    algorithmSelector.value = 'default';
}

// 监听选择算法的事件
algorithmSelector.addEventListener('change', (event) => {
    // 获取选择的算法
    const selectedAlgorithm = event.target.value;
    // 根据选择的算法更新算法简介
    switch (selectedAlgorithm) {
        case 'algorithm1':
            algorithmDescription.textContent = '算法1的简介。';
            break;
        case 'algorithm2':
            algorithmDescription.textContent = '算法2的简介。';
            break;
        case 'algorithm3':
            algorithmDescription.textContent = '算法3的简介。';
            break;
        case 'algorithm4':
            algorithmDescription.textContent = '算法4的简介。';
            break;
        default:
            algorithmDescription.textContent = '请选择一个算法查看相应简介。';
            break;
    }
});



// 获取参数表单
const parameterForm = document.querySelector('.algorithm-parameter');

// 添加事件监听器，当算法选择器的值改变时触发
algorithmSelector.addEventListener('change', function () {
    // 获取选中的算法的值
    var selectedAlgorithm = algorithmSelector.value;

    // 根据选中的算法更新参数表单
    switch (selectedAlgorithm) {
        case 'algorithm1':
            parameterForm.innerHTML = `
                 <div class="card-body bg-light d-flex align-items-center">
                    <div class="form-group me-2">
                        <label for="param1">该算法无需输入参数。</label>
                    </div>
                </div>
            `;
            break;
        case 'algorithm2':
            parameterForm.innerHTML = `
                <div class="card-body bg-light d-flex align-items-center">
                    <div class="form-group me-2 col-5 col-sm-5">
                        靶标类型
                        <select name="markType" id="markType" >
                            <option value="default"></option>
                            <option value="SingleCircle">单孔</option>
                            <option value="CircleMatrix">孔矩阵</option>
                            <option value="ComplexCircles">复合孔</option>
                            <option value="Rectangle">矩形</option>
                            <option value="PlumHoles">梅花孔</option>
                            <option value="Cross">十字</option>
                        </select>
                    </div>
                    <div class="form-group me-2 col-5 col-sm-5">
                        检测算法
                        <select name="algorithm" id="algorithm2" asp-for="SelectedAlgorithm">
                            <option value="default">       </option>
                        </select>
                    </div>
                </div>
                
                    <div class="fw-bold border border-top-1 border-bottom-1 text-center">
                        靶标参数
                    </div>

                    <div>
                        <span class="col-4 ms-3 me-5">参数类型</span>
                        <input class="ms-5" type="radio" name="ParamType" value="px" />像素
                        <input class="ms-5" type="radio" name="ParamType" value="mm" checked />毫米
                    </div>

                     <div>
                        <span class="ms-3">直径</span>
                        <input class="form-control-sm col-3" type="text">
                        <span class="ms-3">误差</span>
                        <input class="form-control-sm col-3" type="text">
                     </div>

                     <div>
                        <span class="ms-3">白色靶标</span>
                        <label class="ms-5"><input type="checkbox"></label>
                     </div>
                     
                     <div id="additionParam">
                     </div>
            `;
            break;
        case 'algorithm3':
            parameterForm.innerHTML = `
                 <div class="card-body bg-light d-flex align-items-center">
                    <div class="form-group me-2">
                        <label for="param1">参数1:</label>
                        <input type="text" class="form-control" id="param1" placeholder="">
                    </div>
                    <div class="form-group me-2">
                        <label for="param2">参数2:</label>
                        <input type="text" class="form-control" id="param2" placeholder="">
                    </div>
                    <div class="form-group">
                        <label for="param3">参数3:</label>
                        <input type="text" class="form-control" id="param3" placeholder="">
                    </div>
                </div>
            `;
            break;
        case 'algorithm4':
            parameterForm.innerHTML = `
                 <div class="card-body bg-light d-flex align-items-center">
                    <div class="form-group me-2">
                        <label for="param1">参数1:</label>
                        <input type="text" class="form-control" id="param1" placeholder="">
                    </div>
                    <div class="form-group me-2">
                        <label for="param2">参数2:</label>
                        <input type="text" class="form-control" id="param2" placeholder="">
                    </div>
                    <div class="form-group me-2">
                        <label for="param3">参数3:</label>
                        <input type="text" class="form-control" id="param3" placeholder="">
                    </div>
                    <div class="form-group">
                        <label for="param4">参数4:</label>
                        <input type="text" class="form-control" id="param4" placeholder="">
                    </div>
                </div>
            `;
            break;
        // 更多算法可以在这里添加
        default:
            // 如果选择的算法不存在或未定义，则清空参数表单
            parameterForm.innerHTML = `
                <div class="card-body bg-light d-flex align-items-center">
                        请选择算法以输入相应参数！
                </div>
            `;
    }
});

//当选择靶标类型不同时，靶标参数也不同
//const parameterForm = document.querySelector('.algorithm-parameter');

// 在父元素上添加事件监听器，使用委托方式处理子元素的事件
parameterForm.addEventListener("change", (event) => {
    const markTypeSelect = document.getElementById("markType");
    const algorithmSelect = document.getElementById("algorithm2");

    // 如果修改的是靶标类型下拉列表，则根据新的选择更新检测算法下拉列表
    if (event.target === markTypeSelect) {
        switch (markTypeSelect.value) {
            case "SingleCircle":
                algorithmSelect.innerHTML = `
          <option value="defect_circle">defect_circle</option>
          <option value="config_circle">config_circle</option>
          <option value="custom_circle">custom_circle</option>
        `;
                break;
            case "CircleMatrix":
                algorithmSelect.innerHTML = `
          <option value="matrixhole">matrixhole</option>
          <option value="config_Matrixhole">config_Matrixhole</option>
          <option value="custom_Matrixhole">custom_Matrixhole</option>
        `;
                additionParam.innerHTML = `
                <div class="bg-light d-flex align-items-center">
                    <div class="form-group me-2">
                        <span class="ms-3">行数</span>
                        <input type="text" class="form-control" id="param1" placeholder="">
                    </div>
                    <div class="form-group me-2">
                        <label for="">列数</label>
                        <input type="text" class="form-control" id="param2" placeholder="">
                    </div>
                </div>
                    <div>
                        <span class="ms-3">行数</span>
                        <input styleclass="form-control form-control-sm col-3" type="text">
                        <span class="ms-3">列数</span>
                        <input class="form-control-sm col-3" type="text">
                     </div>

                    <div>
                        <span class="ms-3">行间距</span>
                        <input class="form-control-sm col-3" type="text">
                        <span class="ms-3">列间距</span>
                        <input class="form-control-sm col-3" type="text">
                     </div>
        `;
                break;
            case "ComplexCircles":
                algorithmSelect.innerHTML = `
          <option value="matrix_hole">matrix_hole</option>
          <option value="rect_hole">rect_hole</option>
          <option value="ring_hole">ring_hole</option>        
          <option value="circle_ring">circle_ring</option>
          <option value="Rhombus">Rhombus</option>          
          <option value="triangle_one">triangle_one</option>
          <option value="border_hole">border_hole</option>
          <option value="complex_plum">complex_plum</option>
        `;
                additionParam.innerHTML = `
                    <div>
                        <span class="ms-3">行数</span>
                        <input class="form-control-sm col-3" type="text">
                        <span class="ms-3">列数</span>
                        <input class="form-control-sm col-3" type="text">
                     </div>

                    <div>
                        <span class="ms-3">行间距</span>
                        <input class="form-control-sm col-3" type="text">
                        <span class="ms-3">列间距</span>
                        <input class="form-control-sm col-3" type="text">
                     </div>

                    <div>
                        <span class="ms-3">宽</span>
                        <input class="form-control-sm col-3" type="text">
                        <span class="ms-3">高</span>
                        <input class="form-control-sm col-3" type="text">
                     </div>

                    <div>
                        <span class="ms-3">内孔直径</span>
                        <input class="form-control-sm col-3" type="text">
                        <span class="ms-3">权重</span>
                        <input class="form-control-sm col-3" type="text">
                     </div>
        `;
                break;
            case "Rectangle":
                algorithmSelect.innerHTML = `
          <option value="rectangle">rectangle</option>
          <option value="Rhombus">Rhombus</option>          
          <option value="custom_Rect">custom_Rect</option>
        `;
                additionParam.innerHTML = `
                    <div>
                        <span class="ms-3">宽</span>
                        <input class="form-control-sm col-3" type="text">
                        <span class="ms-3">高</span>
                        <input class="form-control-sm col-3" type="text">
                     </div>
        `;
                break;
            case "PlumHoles":
                algorithmSelect.innerHTML = `
          <option value="plumhole">plumhole</option>
          <option value="config_Plum">config_Plum</option>
          <option value="custom_Plum">custom_Plum</option>
        `;
                additionParam.innerHTML = `
                    <div>
                        <span class="ms-3">行数</span>
                        <input class="form-control-sm col-3" type="text">
                        <span class="ms-3">列数</span>
                        <input class="form-control-sm col-3" type="text">
                     </div>

                    <div>
                        <span class="ms-3">行间距</span>
                        <input class="form-control-sm col-3" type="text">
                        <span class="ms-3">列间距</span>
                        <input class="form-control-sm col-3" type="text">
                     </div>

                    <div>
                        <span class="ms-3">内孔直径</span>
                        <input class="form-control-sm col-3" type="text">
                     </div>
                break;
        `;
            case "Cross":
                algorithmSelect.innerHTML = `
          <option value="cross">cross</option>
          <option value=custom_cross">custom_cross</option>
          <option value="config_cross">config_cross</option>
        `;
                additionParam.innerHTML = `
                    <div>
                        <span class="ms-3">宽</span>
                        <input class="form-control-sm col-3" type="text">
                        <span class="ms-3">高</span>
                        <input class="form-control-sm col-3" type="text">
                     </div>
        `;
                break;
            default:
                algorithmSelect.innerHTML = `
          <option value="default"></option>
        `;
                break;
        }
    }
});

// 获取按钮
let mybutton = document.getElementById("myBtn");

// 向下滚动页面就显示按钮
window.onscroll = function () { scrollFunction() };

function scrollFunction() {
    if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
        mybutton.style.display = "block";
    } else {
        mybutton.style.display = "none";
    }
}

// 点击按钮回到top
function topFunction() {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
}