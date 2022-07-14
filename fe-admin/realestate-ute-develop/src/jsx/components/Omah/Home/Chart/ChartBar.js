import React from 'react'

import ReactApexChart from 'react-apexcharts'


function ChartBar(props) {
  console.log("chart", props)


  let state = {
    series: [
      {
        name: 'Nhà ở',
        data: props.housePrice,
      },
      {
        name: 'Đất',
        data: props.landPrice,
      },
      {
        name: 'Văn phòng/ Mặt bằng kinh doanh',
        data: props.officePrice,
      },
      {
        name: 'Căn hộ/ Chung cư',
        data: props.appartmentPrice,
      }
    ],
    chart: {
      height: 350,
      type: 'area',
      toolbar: false,
    },
    options: {
      chart: {
        type: 'bar',
        height: 230,
        toolbar: {
          show: false,
        },
      },
      plotOptions: {
        bar: {
          horizontal: false,
          columnWidth: '60%',
          endingShape: 'flat',
        },
      },

      colors: ['#3B4CB8', '#37D159', '#FF9432', '#1EA7C5'],
      legend: {
        show: false,
        position: 'top',
        horizontalAlign: 'left',
      },
      dataLabels: {
        enabled: false,
      },
      stroke: {
        curve: 'smooth',
        width: 5,
        colors: ['#3B4CB8', '#37D159', '#FF9432', '#1EA7C5'],
      },

      markers: {
        size: 0,
        border: 0,
        colors: ['#3B4CB8', '#37D159', '#FF9432', '#1EA7C5'],
        hover: {
          size: 8,
        },
      },
      yaxis: {
        labels: {
          style: {
            colors: '#3e4954',
            fontSize: '14px',
            fontFamily: 'Poppins',
            fontWeight: 100,
          },
          formatter: function (y) {
            return y.toFixed(2) + ' triệu/m2'
          },
        },
      },
      xaxis: {
        type: 'month',
        categories: props.months,
      },
      //colors: ['#3B4CB8', '#37D159'],
      fill: {
        colors: ['#3B4CB8', '#37D159', '#FF9432', '#1EA7C5'],
      },
      tooltip: {
        x: {
          format: 'month',
        },
      },
    },
  }

  return (
    <div id='chart'>
      <ReactApexChart
        options={state.options}
        series={state.series}
        type='area'
        height={350}
      />
    </div>
  )
}

export default ChartBar
