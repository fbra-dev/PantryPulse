import { TestBed } from '@angular/core/testing';

import { SensorViewService } from './sensor-view.service';

describe('SensorViewService', () => {
  let service: SensorViewService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SensorViewService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
