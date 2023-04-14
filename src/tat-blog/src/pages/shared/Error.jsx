import React from 'react';

export default function Error(props) {
  const { message } = props;
  return <div>{message}</div>;
}
