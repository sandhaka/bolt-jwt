import {Component, OnInit, TemplateRef, ViewChild} from '@angular/core';
import {Page} from "../../shared/datatable/model/page";
import {Observable} from "rxjs";
import {PagedData} from "../../shared/datatable/model/paged-data";
import {TokenLogsService} from "./token-logs.service";
import {HttpErrorResponse} from "@angular/common/http";
import {DataTableService} from "../../shared/datatable/data-table.service";
import {BaseChartDirective} from "ng2-charts";

@Component({
  templateUrl: 'dashboard.component.html',
  providers: [DataTableService]
})
export class DashboardComponent implements OnInit {

  /**
   * Charts data
   */

  // Ref to bar chart
  @ViewChild(BaseChartDirective) chart: BaseChartDirective;

  @ViewChild('dateCol') dateCol: TemplateRef<any>;

  // Data series
  barChartData: any[] = [
    {
      data: [],
      label: 'Released tokens'
    }
  ];

  // Labels
  barChartLabels: string[] = [];

  // Chart options
  barChartOptions: any = {
    scaleShowVerticalLines: false,
    responsive: true,
    scales: {
      yAxes: [{
        ticks: {
          beginAtZero:true
        }
      }]
    }
  };

  /**
   * Number of days displayed in the chart
   */
  numberOfDisplayedDays: number;

  /**
   * Number of record (token logs) to show
   */
  numberOfElements: number;

  columnNames = [ ];

  /**
   * Service callbacks ref
   */
  getDataFunc: Function;
  onSelectFunc: Function;

  constructor(private tokenLogsService: TokenLogsService) {
    this.numberOfElements = 16;
    this.numberOfDisplayedDays = 30;
  }

  ngOnInit() {
    // To have working cell template columns must be into the ngOnInit routine
    this.columnNames = [
      {name: 'Timestamp', prop: 'timestamp', cellTemplate: this.dateCol, sortable: true},
      {name: 'UserId', prop: 'userId', sortable: true}
    ];

    // Bind
    this.getDataFunc = this.getData.bind(this);
    this.onSelectFunc = this.onSelect.bind(this);
  }

  /**
   * The service callback
   * @param params
   * @returns {Observable<PagedData<any>>}
   */
  getData(params: Page): Observable<PagedData<any>> {
    const observable = this.tokenLogsService.getPaged(params);

    // Keep a chart data copy
    observable.subscribe(
      (pagedData: PagedData<any>) => {
        this.setupChartSerie(pagedData.data);
      },
      (errorResponse: HttpErrorResponse) => {
        // Just handled by the data table component
      }
    );

    // Return to data table input
    return observable;
  }

  private setupChartSerie(data: any[]) {
    // Get data of interest
    const dataTimes = data.map(x => [ new Date(x.timestamp), x.userId ] );

    // Build the x axes
    const xAxes = [];
    const now = Date.now() - 86400000 * this.numberOfDisplayedDays;

    for(let i = 1; i <= this.numberOfDisplayedDays; i++) {
      const d = new Date( now + 86400000 * i);
      xAxes.push(new Date(d.getFullYear(), d.getMonth(), d.getDate(), 0, 0, 0));
    }

    // Complete the chart values
    const yValues: number[] = [];
    const xValues: string[] = [];

    for(let j = 0; j < xAxes.length; j++) {

      // Show the number of tokens released for each day
      const values = dataTimes.filter((item) => {
        return item[0].getFullYear() === xAxes[j].getFullYear() &&
          item[0].getMonth() === xAxes[j].getMonth() &&
          item[0].getDate() === xAxes[j].getDate();
      });

      // Push into the temporary array
      yValues.push(values.length);
      xValues.push(xAxes[j].toLocaleDateString());
    }

    // Update chart
    const clone = JSON.parse(
      JSON.stringify(this.barChartData)
    );
    clone[0].data = yValues;
    this.barChartData = clone;

    this.chart.chart.config.data.labels = xValues;
    this.chart.chart.update();
  }

  onSelect({selected}) {

  }
}
