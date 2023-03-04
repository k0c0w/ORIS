import { Space, Row, Col, Card, Pagination } from 'antd';
import { useEffect } from 'react';

import { NavLink } from 'react-router-dom';


const { Meta } = Card;



const pageSizeOptions = [5, 10, 50, 100];

 const DogsList = ({dogs, totalDogs, itemsPerPage, page, setItemsPerPage, setPage}) =>
 {
    useEffect(() => { const href = window.location.href;
                      const indexOfId = href.indexOf("#");
                      const elem = href.substring(indexOfId+1);
                      document.getElementById(elem)?.scrollIntoView();
                    })
    return   (
    <Space
      direction="vertical"
      style={{
        width: '100%',
        height: '100%'
      }}
      size={[0, 48]}
    >
          <Row gutter={[16, 16]} justify={"space-between"}>
          {dogs.map(element => (
            <Col style={{padding: 20}}>
              <NavLink to={`/breeds/${element.id}`}>
                <Card
                  id={element.id}
                  hoverable
                  style={{ width: 240, minHeight:300 }}
                  cover={<img alt="dog" style={{maxHeight: 200}}src={element.image_url} />}>
    
                    <Meta title={element.name} description={element.breed_group}></Meta>
                </Card>
              </NavLink>
            </Col>
          ))}
          </Row>
          
          <div>
          <Pagination
              style={{position:'absolute'}}
              total={totalDogs}
              showTotal={(totalDogs) => `Total ${totalDogs} breeds`}
              pageSize={itemsPerPage}
              current={page}
              onChange={(page, elementsOnPage) => {setPage(page); setItemsPerPage(elementsOnPage);}}
              onShowSizeChange={(size) => setItemsPerPage(size)}
              pageSizeOptions={pageSizeOptions}
          />
          <NavLink style={{position: "absolute", right: 10}} to="/review">Leave review about our service</NavLink>
          </div>

    </Space>
    );
 }

export default DogsList;