import React, { useEffect } from 'react';

function Rss() {
  useEffect(() => {
    document.title = 'Rss';
  }, []);
  return <div>Rss</div>;
}

export default Rss;
