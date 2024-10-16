$(document).ready(function(){
    // model 框显示隐藏
    $(".sl_chioce_sele button").on('click',function(){
        $('#myModal').modal();
        // feedback
        // debugger;
        $('input').attr("checked",false);
    })
    $(".fa-edit").on('click',function(){
        $('#feedback').modal();
        // $('input').attr("checked",false);
    })

    //确认按钮 
    $('#btn_submit').click(function(){
        let Arr = '';
        $('.sl_zhibiao li').each(function(){
            // debugger;
            let flag = $(this).children().find('input').is(':checked');
            let Ele = $(this).children().find('input').next();
            // console.log(flag)
            if(flag){
                var ele = Ele.text();
                Arr[Arr.length] = ele;
                console.log(Arr)
            }
        })
        // alert(Arr)
    })
    // $("#testtable").bootstrapTable({
    //                    columns: [{
    //                        field: 'id',
    //                        title: 'Item ID'
    //                    }, {
    //                        field: 'name',
    //                        title: 'Item Name'
    //                    },{
    //                        field: 'price',
    //                        title: 'Item Price'
    //                    }],
    //                    data: [{ //数据
    //                        id: 1,
    //                        name: 'Item 1',
    //                        price: '$1'
    //                    }, {
    //                        id: 2,
    //                        name: 'Item 2',
    //                        price: '$2'
    //                    }, {
    //                        id: 3,
    //                        name: 'Item 3',
    //                        price: '$2'
    //                    }, {
    //                        id: 4,
    //                        name: 'Item 4',
    //                        price: '$2'
    //                    }]
    //                });
})
//时间左右切换
function sl_Left(){
    let newText = '';
    let ele = document.getElementsByClassName('sl_chioDate')[0].children[1].firstChild;
    let oldText = document.getElementsByClassName('sl_chioDate')[0].children[1].firstChild.text;
    if(oldText=='今天'){
        ele.innerText='昨天'
    }
}
function sl_Right(){
    let newText = '';
    let ele = document.getElementsByClassName('sl_chioDate')[0].children[1].firstChild;
    let oldText = document.getElementsByClassName('sl_chioDate')[0].children[1].firstChild.text;
    if(oldText=='昨天'){
        ele.innerText='今天'
    }
}

//创建表格
//  echarts 图表 
//  echarts 泵房类型统计
var sl_ech_type = echarts.init(document.getElementById('sl_ech_type'));
sl_ech_type.setOption({
    tooltip: {
        trigger: 'item',
        formatter: '{a} <br/>{b}: {c} ({d}%)'
    },
    legend: {
        orient: 'vertical',
        left: 10,
        data: ['常规泵房', '调峰泵房', '二次供水泵房', '微型泵房', '其他泵房']
    },
    series: [
        {
            name: '访问来源',
            type: 'pie',
            radius: ['50%', '70%'],
            avoidLabelOverlap: false,
            label: {
                normal: {
                    show: false,
                    position: 'center'
                },
                emphasis: {
                    show: true,
                    textStyle: {
                        fontSize: '14',
                        fontWeight: 'bold'
                    }
                }
            },
            labelLine: {
                normal: {
                    show: false
                }
            },
            data: [
                {value: 335, name: '常规泵房',itemStyle:{
                    color:'#2c7afa'
                }},
                {value: 310, name: '调峰泵房',itemStyle:{
                    color:'#15a3f9'
                }},
                {value: 234, name: '二次供水泵房',itemStyle:{
                    color:'#25cfe9'
                }},
                {value: 135, name: '微型泵房',itemStyle:{
                    color:'#04ccae'
                }},
                {value: 1548, name: '其他泵房',itemStyle:{
                    color:'#78dca9'
                }}
            ]
        }
    ]
})

//泵房使用年限统计
var sl_ech_time = echarts.init(document.getElementById('sl_ech_time'));
sl_ech_time.setOption({
    color: ['#3398DB'],
    tooltip: {
        trigger: 'axis',
        axisPointer: {            // 坐标轴指示器，坐标轴触发有效
            type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
        }
    },
    grid: {
        left: '3%',
        right: '4%',
        bottom: '3%',
        containLabel: true
    },
    xAxis: [
        {
            type: 'category',
            data: ['1年以内', '1到2年', '2到5年', '5到10年', '10年以上'],
            axisTick: {
                alignWithLabel: true
            }
        }
    ],
    yAxis: [
        {
            type: 'value'
        }
    ],
    series: [
        {
            name: '泵房',
            type: 'bar',
            barWidth: '20%',
            data: [10, 4, 3, 14, 3, 2, 26]
        }
    ]
})
//机组水泵品牌统计
var sl_ech_brand = echarts.init(document.getElementById('sl_ech_brand'));
sl_ech_brand.setOption({
    dataset: {
        source: [
            ['score', '数量', 'product'],
            [89.3, 2, '格兰富'],
            [57.1, 1, '东方'],
            [89.7, 2, '安邦'],
            [68.1, 7, '三利'],
            [19.6, 1, 'Orange'],
            [32.7, 1, 'Walnut']
        ]
    },
    grid: {containLabel: true},
    xAxis: {name: '数量'},
    yAxis: {type: 'category'},
    visualMap: {
        show:false,
        left: 'center',
        min: 10,
        max: 100,
        text: ['High Score', 'Low Score'],
        // Map the score column to color
        dimension: 0,
        inRange: {
            color: ['#2c7afa', '#78dca9']
        }
    },
    series: [
        {
            type: 'bar',
            barWidth: '50%',
            encode: {
                // Map the "amount" column to X axis.
                x: '数量',
                // Map the "product" column to Y axis
                y: 'product'
            }
        }
    ]
})
//泵房厂家统计
var sl_ech_business =echarts.init(document.getElementById('sl_ech_business'));
sl_ech_business.setOption({
    tooltip: {
        trigger: 'item',
        formatter: '{a} <br/>{b}: {c} ({d}%)'
    },
    legend: {
        orient: 'vertical',
        left: 10,
        data: ['三利', '东方', '安邦', '格兰富', 'Orange']
    },
    series: [
        {
            name: '访问来源',
            type: 'pie',
            radius: ['50%', '70%'],
            avoidLabelOverlap: false,
            label: {
                normal: {
                    show: false,
                    position: 'center'
                },
                emphasis: {
                    show: true,
                    textStyle: {
                        fontSize: '14',
                        fontWeight: 'bold'
                    }
                }
            },
            labelLine: {
                normal: {
                    show: false
                }
            },
            data: [
                {value: 335, name: '三利',itemStyle:{
                    color:'#2c7afa'
                }},
                {value: 310, name: '东方',itemStyle:{
                    color:'#15a3f9'
                }},
                {value: 234, name: '安邦',itemStyle:{
                    color:'#25cfe9'
                }},
                {value: 135, name: '格兰富',itemStyle:{
                    color:'#04ccae'
                }},
                {value: 1548, name: 'Orange',itemStyle:{
                    color:'#78dca9'
                }}
            ]
        }
    ]
})
//泵房质保情况统计图表
var sl_ech_quality = echarts.init(document.getElementById('sl_ech_quality'));
sl_ech_quality.setOption({
    color: ['#25cfe9'],
    tooltip: {
        trigger: 'axis',
        axisPointer: {            // 坐标轴指示器，坐标轴触发有效
            type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
        }
    },
    grid: {
        left: '3%',
        right: '4%',
        bottom: '3%',
        containLabel: true
    },
    xAxis: [
        {
            type: 'category',
            data: ['半年以内', '0.5-1年', '1到2年', '2到3年', '3年以上'],
            axisTick: {
                alignWithLabel: true
            }
        }
    ],
    yAxis: [
        {
            type: 'value'
        }
    ],
    series: [
        {
            name: '泵房',
            type: 'bar',
            barWidth: '20%',
            data: [10, 4, 3, 14, 3, 2, 26]
        }
    ]
});   
// 指定图表的配置项和数据
var sl_ech_equipment = echarts.init(document.getElementById('sl_ech_equipment'));
sl_ech_equipment.setOption({
tooltip: {
    trigger: 'item',
    formatter: '{a} <br/>{b}: {c} ({d}%)'
},
legend: {
    orient: 'vertical',
    left: 10,
    data: ['厢式变频', '厢式叠压', '加压泵站', '格式叠压', 'Orange']
},
series: [
    {
        name: '访问来源',
        type: 'pie',
        radius: ['50%', '70%'],
        avoidLabelOverlap: false,
        label: {
            normal: {
                show: false,
                position: 'center'
            },
            emphasis: {
                show: true,
                textStyle: {
                    fontSize: '14',
                    fontWeight: 'bold'
                }
            }
        },
        labelLine: {
            normal: {
                show: false
            }
        },
        data: [
            {value: 335, name: '厢式变频',itemStyle:{
                color:'#2c7afa'
            }},
            {value: 310, name: '厢式叠压',itemStyle:{
                color:'#15a3f9'
            }},
            {value: 234, name: '加压泵站',itemStyle:{
                color:'#25cfe9'
            }},
            {value: 135, name: '格式叠压',itemStyle:{
                color:'#04ccae'
            }},
            {value: 1548, name: 'Orange',itemStyle:{
                color:'#78dca9'
            }}
        ]
    }
]
});
window.onresize = function(){
sl_ech_equipment.resize();
sl_ech_quality.resize();
sl_ech_business.resize();
sl_ech_brand.resize();
sl_ech_time.resize();
sl_ech_type.resize();
}

















