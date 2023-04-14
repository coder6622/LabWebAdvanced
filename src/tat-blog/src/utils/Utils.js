import React from 'react';
import { useLocation } from 'react-router-dom';

export function isEmptyOrSpaces(str) {
  return str == null || (typeof src === 'string' && (str.match(/^ *$/) !== null || str.length === 0));
}

export function useMyQuery() {
  const { search } = useLocation();

  return React.useMemo(() => new URLSearchParams(search), [search]);
}

export function getMonthName(monthNumber) {
  const date = new Date();
  date.setMonth(monthNumber - 1);

  return date.toLocaleString('en-US', {
    month: 'short',
  });
}

export const monthsLong = [
  { nameMonth: 'Tháng 1', value: 1 },
  { nameMonth: 'Tháng 2', value: 2 },
  { nameMonth: 'Tháng 3', value: 3 },
  { nameMonth: 'Tháng 4', value: 4 },
  { nameMonth: 'Tháng 5', value: 5 },
  { nameMonth: 'Tháng 6', value: 6 },
  { nameMonth: 'Tháng 7', value: 7 },
  { nameMonth: 'Tháng 8', value: 8 },
  { nameMonth: 'Tháng 9', value: 9 },
  { nameMonth: 'Tháng 10', value: 10 },
  { nameMonth: 'Tháng 11', value: 11 },
  { nameMonth: 'Tháng 12', value: 12 },
];
